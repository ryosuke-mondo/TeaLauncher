﻿/*
 * TeaLauncher. Simple command launcher.
 * Copyright (C) Toshiyuki Hirooka <toshi.hirooka@gmail.com> http://wasabi.in/
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */


using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CommandLauncher
{

    public partial class MainWindow
        : Form
        , ICommandManagerInitializer
        , ICommandManagerFinalizer
        , ICommandManagerDialogShower
    {
        // コマンド管理
        CommandManager m_CommandManager;

        // 読み込んだコンフィグのファイル名
        string m_ConfigFileName;

        // ホットキー
        HotKey m_Hotkey;

        string LICENSE = @"TeaLauncher. Simple command launcher.
Copyright (C) Toshiyuki Hirooka <toshi.hirooka@gmail.com> http://wasabi.in/

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.";

    
        public MainWindow(string conf_filename)
        {
            m_CommandManager = new CommandManager(this, this, this);

            InitializeComponent();

            try
            {
                // 設定を読み込んで反映する
                InitializeByConfigFile(conf_filename);
            }
            catch (MainWindowNotExistsLinkToException e)
            {
                MessageBox.Show(e.Message + "\n" + "filename : "+ conf_filename, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

#if DEBUG
            m_Hotkey = new HotKey(MOD_KEY.ALT, Keys.Space);
#else
            m_Hotkey = new HotKey(MOD_KEY.CONTROL, Keys.Space);
#endif
            m_Hotkey.HotKeyPressed += new EventHandler(PressHotkey);
        }

        void InitializeByConfigFile(string filename)
        {
            m_ConfigFileName = filename;
            m_CommandManager.ClearCommands();

            ConfigLoader conf = new ConfigLoader();
            conf.LoadConfigFile(filename);
            List<string> sections = conf.GetSections();
            foreach (string section in sections)
            {
                Hashtable hashtable = conf.GetConfig(section);
                Command cmd = new Command();
                cmd.command = section;
                cmd.execution = (string)hashtable["linkto"];
                if (cmd.execution == null)
                    throw new MainWindowNotExistsLinkToException();
                m_CommandManager.RegisterCommand(cmd);
            }
        }

        public void Reinitialize()
        {
            InitializeByConfigFile(m_ConfigFileName);
        }

        public void Exit()
        {
            Application.Exit();
            m_Hotkey.Dispose();
        }

        public void ShowVersionInfo()
        {
            System.Diagnostics.FileVersionInfo version =
                System.Diagnostics.FileVersionInfo.GetVersionInfo(
                System.Reflection.Assembly.GetExecutingAssembly().Location);

            MessageBox.Show(version.FileDescription + " version " + version.FileVersion + "\n" +
                "--------------------------------" + "\n" +
                LICENSE, version.FileDescription);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message);
        }

        void PressHotkey(object sender, EventArgs e)
        {
            // 非表示なら表示してアクティブにする。アクティブなら非アクティブにして非表示にする。
            if (IsWindowShown())
            {
                if (Form.ActiveForm != this)
                {
                    this.Activate();
                }
                else
                {
                    HideWindow();
                }
            }
            else
            {
                ShowWindow();
                this.Activate();
            }
        }

        bool IsWindowShown()
        {
            return this.Visible;
        }

        private void ShowWindow()
        {
            this.Show();

            IMEController ime = new IMEController();
            ime.Off();
            ime.On();
            ime.Off();
        }

        private void HideWindow()
        {
            this.Hide();
            ClearCommandBox();
        }

        private void command_box_PreviewKeyDown_1(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                case Keys.Tab:
                    e.IsInputKey = true;
                    break;
            }
        }

        private void command_box_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Tab:
                    {
                        e.Handled = true;

                        // 補完する
                        CompleteCommandBox();
                    }
                    break;

                case Keys.Escape:
                    {
                        e.Handled = true;

                        if (command_box.Text != "")
                        {
                            ClearCommandBox();
                        }
                        else
                        {
                            HideWindow();
                        }
                    }
                    break;

                case Keys.Enter:
                    {
                        e.Handled = true;

                        RunCommand(command_box.Text);
                    }
                    break;
                case Keys.F5: 
                    {
                        e.Handled = true;

                        Reinitialize();
                    }
                    break;
                case Keys.F4: {
                        e.Handled = true;

                        RunCommand(".\\" + m_ConfigFileName);
                    }
                    break;
            }
        }

        private void CompleteCommandBox()
        {
            // 補完候補など取得
            string input_str = this.command_box.Text;
            IEnumerable<string> candidates = m_CommandManager.GetCandidates(input_str);
            string compl_str = m_CommandManager.AutoCompleteWord(input_str);

            // 候補があればコマンドボックスを補完する
            if (candidates.Any())
            {
                // 候補が2つ以上あるなら
                if (candidates.Count() >= 2)
                {
                    // コンボボックスに候補を表示する
                    command_box.Items.Clear();
                    command_box.DroppedDown = true;
                    command_box.Items.AddRange(candidates.ToArray());
                }
                else
                {
                    ClearDropBox();
                }

                // 補完文字列を表示
                command_box.Text = compl_str;
                command_box.SelectionStart = command_box.Text.Length;
            }
            else
            {
                ClearDropBox();
            }
        }

        private void ClearCommandBox()
        {
            // クリアする
            ClearDropBox();
            command_box.Text = "";
        }

        private void ClearDropBox()
        {
            // クリアする
            command_box.DroppedDown = false;
            command_box.Items.Clear();
            command_box.SelectionStart = command_box.Text.Length; // Items.Clear するとおかしくなるから
        }

        private void RunCommand(string command)
        {
            if (m_CommandManager.HasCommand(command))
            {
                Debug.WriteLine("Run : " + command);

                ClearCommandBox();
                HideWindow();

                try
                {
                    m_CommandManager.Run(command);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            // 起動時は非表示
            HideWindow();
        }

        private void command_box_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }


    class MainWindowNotExistsLinkToException
        : Exception
    {
        public override string Message
        {
            get { return "設定ファイルに \"linkto\" キーが存在しません。"; }
        }
    }
}
