using PS3Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PWRTM
{
    public partial class MainForm : Form
    {
        private bool Busy = false;
        private const bool Debug = false;
        public readonly CCAPI PS3 = new CCAPI();
        private uint _soldierCount;
        private List<Soldier> _soldierList = new List<Soldier>();
        private Soldier _snake;

        /// <summary>A workaround for the combobox being stupid with the index sometimes.</summary>
        private int _indexcounter = 0;

        public MainForm()
        {
            InitializeComponent();
            JobBox.DataSource = Enums.Jobs;
            TypeBox.DataSource = Enums.Type;
            Skill1.DataSource = Enums.Skills.Keys.ToList();
            Skill2.DataSource = Enums.Skills.Keys.ToList();
            Skill4.DataSource = Enums.Skills.Keys.ToList();
            Skill3.DataSource = Enums.Skills.Keys.ToList();
        }

        private async void Connect(object sender, EventArgs e)
        {
            SetStatus("Attempting Connection...");

            bool d = false;
            await Task.Run(() =>
            {
                try
                {
                    d = (IPAddress.TryParse(IPBox.Text, out IPAddress ip))
                        ? (PS3.SUCCESS(PS3.ConnectTarget(ip.ToString())))
                        : ((PS3.ConnectTarget()));
                }
                catch (Exception ex)
                {
                    // Connection error.
                    MessageBox.Show(ex.ToString());
                    SetStatus("Connection Failed!");
                }
            });
            if (!d) return;
            SetStatus("Connection Established.");
            PS3.GetProcessList(out uint[] procs);
            // Get a process name
            PS3.GetProcessName(procs[0], out string name); // Return the name of the process 0.
            if (!name.Contains("_main_MGS_PW.self"))
            {
                MessageBox.Show("Connection Established But Peace Walker Process Not Detected", "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                SetStatus("Attachment Failed, Game Process Not Found!");
                return;
            }
            // Attach your desired process
            if (PS3.SUCCESS(PS3.AttachProcess(procs[0])))
            {
                SetStatus("Attached To Game Process.");
                GetSoldiers();
            }
            else
            {
                SetStatus("Attachment Failed!");
            }
        }

        /// <summary> Gets Soldier Count & Scans All Soldiers </summary>
        private async void GetSoldiers()
        {
            //Snake is special and does not work like other soldiers, exempt from the lists and has the soldier list header after him.
            //GET SNAKE
            //SetStatus("Reading SNAKE.");
            //var b = PS3.GetBytes(0x0131CF98, 0xA0);
            //_snake = new Soldier(ref b);
            //SolCom.Items.Add(Snake.Name);

            //Get SoldierCount
            SetStatus("Counting Soldiers...");
            _soldierCount = PS3.Extension.ReadUInt32(0x0131D030);
            solcount.Text = String.Format("Soldiers: {0}", _soldierCount);
            SetStatus("Soldiers Counted.");
            //Get Soldiers
            SolCom.Items.Clear();

            if (_soldierCount == 0) return; //No gamesave loaded
#if DEBUG
            _soldierCount = 25;//Debug
#endif
            PS3.Notify(CCAPI.NotifyIcon.INFO, "Scanning Soldiers.");
            SetStatus("Commencing Soldier Scan...");
            Busy = true;
            //Experimental multi-soldier grab for performance gains.
            //By pulling multiple soldiers at once and parsing them locally we reduce strain on ccapi, which is unstable from multiple peeks.
            var g = (_soldierCount / 10);
            await Task.Run(() =>
             {
                 for (int i = 0; i < g; i++)
                 {
                     //Starting offset * ((0xA0*0xA) *i)
                     var o = (uint)((0x0131D058 + (0x640 * i)));
                     SetStatus(String.Format("Scanning Soldier Group: {0}/{1}", i, g));
                     var b = PS3.GetBytes(o, 0x640);
                     for (int j = 0; j < 10; j++)
                     {
                         var sol = new byte[0xA0];
                         Buffer.BlockCopy(b, 0xA0 * j, sol, 0, 0xA0);
                         _soldierList.Add(new Soldier(ref sol));
                         SolCom.Invoke(new MethodInvoker(() => SolCom.Items.Add(_soldierList[_soldierList.Count - 1].Name)));
                     }
                 }

                 var r = _soldierCount - (g * 10);
                 if (r > 0)
                 {
                     var o = 0x0131D058 + ((0x640 * g));
                     var b = PS3.GetBytes(o, 0xA0 * r);
                     for (int j = 0; j < r; j++)
                     {
                         SetStatus(String.Format("Scanning Soldier: {0}/{1}", (g * 10) + j, (g * 10) + r));
                         var sol = new byte[0xA0];
                         Buffer.BlockCopy(b, 0xA0 * j, sol, 0, 0xA0);
                         _soldierList.Add(new Soldier(ref sol));
                         SolCom.Invoke(new MethodInvoker(() => SolCom.Items.Add(_soldierList[_soldierList.Count - 1].Name)));
                     }
                 }
                 SetStatus("Soldiers Scanned, Ready.");
             });
            //One by one method, high count causes crash.
            //await Task.Run(() =>
            //{
            //    for (int i = 0; i < _soldierCount; i++)
            //    {
            //        var o = (uint)((0x0131D058 + (0xA0 * i)));
            //        SetStatus(String.Format("Scanning Soldier: {0}/{1}", i, _soldierCount));
            //        var b = PS3.GetBytes(o, 0xA0);
            //        _soldierList.Add(new Soldier(ref b));
            //        SolCom.Invoke(new MethodInvoker(() => SolCom.Items.Add(_soldierList[i].Name)));
            //    }
            //    SetStatus("Soldiers Scanned, Ready.");
            //});
            Busy = false;
        }

        private void ConnectEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Connect(sender, e);
        }

        private void SolShift2(object sender, EventArgs e)
        {
            try
            {
                _indexcounter = SolCom.SelectedIndex;
                var s = _soldierList[_indexcounter];
                TypeBox.DataBindings.Clear();
                TypeBox.DataBindings.Add(new Binding("SelectedIndex", s, "Type", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                JobBox.DataBindings.Clear();
                JobBox.DataBindings.Add(new Binding("SelectedIndex", s, "Job", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                Skill1.DataBindings.Clear();
                Skill1.DataBindings.Add(new Binding("SelectedItem", s, "Skill1", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                Skill2.DataBindings.Clear();
                Skill2.DataBindings.Add(new Binding("SelectedItem", s, "Skill2", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                Skill3.DataBindings.Clear();
                Skill3.DataBindings.Add(new Binding("SelectedItem", s, "Skill3", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                Skill4.DataBindings.Clear();
                Skill4.DataBindings.Add(new Binding("SelectedItem", s, "Skill4", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                GMPBox.DataBindings.Clear();
                GMPBox.DataBindings.Add(new Binding("Value", s, "GMP", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                //HPbox.DataBindings.Clear();
                //HPbox.DataBindings.Add(new Binding("Value", s, "HP", false,
                //    DataSourceUpdateMode.OnPropertyChanged));
                HP2box.DataBindings.Clear();
                HP2box.DataBindings.Add(new Binding("Value", s, "HPMax", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                //PsycheBox.DataBindings.Clear();
                //PsycheBox.DataBindings.Add(new Binding("Value", s, "Psyche", false,
                //    DataSourceUpdateMode.OnPropertyChanged));
                Psyche2Box.DataBindings.Clear();
                Psyche2Box.DataBindings.Add(new Binding("Value", s, "PsycheMax", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                shootbox.DataBindings.Clear();
                shootbox.DataBindings.Add(new Binding("Value", s, "Shoot", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                throwbox.DataBindings.Clear();
                throwbox.DataBindings.Add(new Binding("Value", s, "Throw", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                defensebox.DataBindings.Clear();
                defensebox.DataBindings.Add(new Binding("Value", s, "Defense", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                walkbox.DataBindings.Clear();
                walkbox.DataBindings.Add(new Binding("Value", s, "Walk", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                placebox.DataBindings.Clear();
                placebox.DataBindings.Add(new Binding("Value", s, "Place", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                fightbox.DataBindings.Clear();
                fightbox.DataBindings.Add(new Binding("Value", s, "Fight", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                runbox.DataBindings.Clear();
                runbox.DataBindings.Add(new Binding("Value", s, "Run", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                reloadbox.DataBindings.Clear();
                reloadbox.DataBindings.Add(new Binding("Value", s, "Reload", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                rdbox.DataBindings.Clear();
                rdbox.DataBindings.Add(new Binding("Value", s, "RnD", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                messbox.DataBindings.Clear();
                messbox.DataBindings.Add(new Binding("Value", s, "Mess", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                medbox.DataBindings.Clear();
                medbox.DataBindings.Add(new Binding("Value", s, "Med", false,
                    DataSourceUpdateMode.OnPropertyChanged));
                intbox.DataBindings.Clear();
                intbox.DataBindings.Add(new Binding("Value", s, "Intel", false,
                    DataSourceUpdateMode.OnPropertyChanged));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region EnterPresses

        private async void NameEnter(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;
                //Padding is to wipe the empty space, lest some of the old name remains.
                var b = Encoding.ASCII.GetBytes(SolCom.Text.PadRight(0x10, Convert.ToChar(0x0)));
                var offset = Setoffset(0x8);
                await Task.Factory.StartNew(() =>
                PS3.SetMemory(offset, b));
                SolCom.Items[_indexcounter] = SolCom.Text;
                SetStatus("Name Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private async void GMPEnter(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode != Keys.Enter) return;
                var offset = Setoffset(0x20);
                var b = BitConverter.GetBytes((uint)GMPBox.Value).Reverse().ToArray();
                await Task.Factory.StartNew(() =>
                PS3.SetMemory(offset, b));
                SetStatus("GMP Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetName_Click(object sender, EventArgs e)
        {
            NameEnter(null, new KeyEventArgs(Keys.Enter));
        }

        private void GMPbtn_Click(object sender, EventArgs e)
        {
            GMPBox.Value = 99999;
            GMPEnter(null, new KeyEventArgs(Keys.Enter));
        }

        #endregion EnterPresses

        #region utilities

        private void SetStatus(string text)
        {
            if (status.GetCurrentParent().InvokeRequired)
                status.GetCurrentParent().Invoke(new MethodInvoker(() => SetStatus(text)));
            else
            {
                status.Text = (String.Format("Status: {0}", text));
            }
        }

        private uint Setoffset(uint shift)
        {
            //0x0131D058 is the start of soldier 1 after SNAKE.
            return (uint)((0x0131D058 + shift) + (0xA0 * _indexcounter));
        }

        #endregion utilities

        private void MaxParameters(object sender, EventArgs e)
        {
            foreach (var n in combatgroup.Controls.OfType<NumericUpDown>()) { n.Value = 1250; }
            foreach (var n in statgroup.Controls.OfType<NumericUpDown>()) { n.Value = 1250; }
            var b = new byte[32];
            for (int index = 0; index < b.Length; index++)
            {
                b[index] = 5;
            }
            SetStatus("All parameters ranked S.");
            //PS3.SetMemory(Setoffset(0x3A), b);
        }

        private void UpdateSoldier(object sender, EventArgs e)
        {
            PS3.Notify(CCAPI.NotifyIcon.INFO, String.Format("Receiving {0} Updates.", _soldierList[_indexcounter].Name));
            SetStatus("Updating Soldier.");
            PS3.SetMemory((uint)(0x0131D058 + (0xA0 * _indexcounter)), _soldierList[_indexcounter].Buffer);
            SetStatus("Soldier Updated.");
            PS3.Notify(CCAPI.NotifyIcon.INFO, _soldierList[_indexcounter].Name + " Updated.");
        }

        private async void ItemUnlock(object sender, EventArgs e)
        {
            //Stealth Camo
            //0 01346928 01
            //0 01346933 03 <- 3 = Unlocked.
            await Task.Run(() =>
            {
                const ulong o = 0x1344a5b;
                byte[] b = { 03 };
                //byte[] b = {03,00,00,00,64,00,00,00,01,00,00,00,00,00,01};
                PS3.Notify(CCAPI.NotifyIcon.INFO, "Unlocking Items & Weapons");
                for (int i = 0; i < 0x017C; i++)
                {
                    SetStatus("Unlocking Item:" + (i + 1));
                    PS3.SetMemory((o + (ulong)(0x18 * i)), b);
                }
                SetStatus("Unlock Complete");
                PS3.Notify(CCAPI.NotifyIcon.INFO, "Unlock Complete.");
            });

            //0x017c
        }

        private void SetSkill(object sender, EventArgs e)
        {
            var b = new byte[4];
            Enums.Skills.TryGetValue(Skill1.SelectedItem.ToString(), out int i);
            b[0] = (byte)i;
            Enums.Skills.TryGetValue(Skill2.SelectedItem.ToString(), out i);
            b[1] = (byte)i;
            Enums.Skills.TryGetValue(Skill3.SelectedItem.ToString(), out i);
            b[2] = (byte)i;
            Enums.Skills.TryGetValue(Skill4.SelectedItem.ToString(), out i);
            b[3] = (byte)i;
            PS3.SetMemory(Setoffset(0x80), b);
        }

        private void RefreshSoldier(object sender, EventArgs e)
        {
            if (Busy) return;
            var o = (uint)((0x0131D058 + (0xA0 * _indexcounter)));
            var b = PS3.GetBytes(o, 0xA0);
            _soldierList[_indexcounter] = (new Soldier(ref b));
            SetStatus(String.Format("Soldier {0} Refreshed", _soldierList[_indexcounter].Name));
        }

        private void HPLots_Click(object sender, EventArgs e)
        {
            HP2box.Value = 9999;
        }

        private void PsycheLots_Click(object sender, EventArgs e)
        {
            Psyche2Box.Value = 9999;
        }

        private void GMPbtn_Click_1(object sender, EventArgs e)
        {
            GMPBox.Value = 99999;
        }

        private async void SuperSoldiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                int counter = 0;
                foreach (var s in _soldierList)
                {
                    s.Defense = 1285;
                    s.Shoot = 1285;
                    s.Run = 1285;
                    s.RnD = 1280;
                    s.Mess = 1280;
                    s.Med = 1280;
                    s.Place = 1285;
                    s.Throw = 1285;
                    s.Fight = 1285;
                    s.Walk = 1285;
                    s.Reload = 1285;
                    s.Intel = 1285;
                    s.HPMax = 9999;
                    s.PsycheMax = 9999;
                    s.GMP = 99999;
                    PS3.SetMemory((uint)((0x0131D058 + (0xA0 * counter))), s.Buffer);
                    SetStatus(String.Format("Writing: {0} #{1}", s.Name, counter));
                    counter++;
                }
            });
        }
    }
}