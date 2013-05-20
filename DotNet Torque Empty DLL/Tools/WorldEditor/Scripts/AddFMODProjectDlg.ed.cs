﻿// Copyright (C) 2012 Winterleaf Entertainment L,L,C.
// 
// THE SOFTW ARE IS PROVIDED ON AN “ AS IS” BASIS, WITHOUT W ARRANTY OF ANY KIND,
// INCLUDING WITHOUT LIMIT ATION THE W ARRANTIES OF MERCHANT ABILITY, FITNESS
// FOR A PARTICULAR PURPOSE OR NON-INFRINGEMENT . THE ENTIRE RISK AS TO THE
// QUALITY AND PERFORMANCE OF THE SOFTW ARE IS THE RESPONSIBILITY OF LICENSEE.
// SHOULD THE SOFTW ARE PROVE DEFECTIVE IN ANY RESPECT , LICENSEE AND NOT LICEN -
// SOR OR ITS SUPPLIERS OR RESELLERS ASSUMES THE ENTIRE COST OF AN Y SERVICE AND
// REPAIR. THIS DISCLAIMER OF W ARRANTY CONSTITUTES AN ESSENTIAL PART OF THIS
// AGREEMENT. NO USE OF THE SOFTW ARE IS AUTHORIZED HEREUNDER EXCEPT UNDER
// THIS DISCLAIMER.
// 
// The use of the WinterLeaf Entertainment LLC DotNetT orque (“DNT ”) and DotNetT orque
// Customizer (“DNTC”)is governed by this license agreement (“ Agreement”).
// 
// R E S T R I C T I O N S
// 
// (a) Licensee may not: (i) create any derivative works of DNTC, including but not
// limited to translations, localizations, technology add-ons, or game making software
// other than Games; (ii) reverse engineer , or otherwise attempt to derive the algorithms
// for DNT or DNTC (iii) redistribute, encumber , sell, rent, lease, sublicense, or otherwise
// transfer rights to  DNTC; or (iv) remove or alter any tra demark, logo, copyright
// or other proprietary notices, legends, symbols or labels in DNT or DNTC; or (iiv) use
// the Software to develop or distribute any software that compete s with the Software
// without WinterLeaf Entertainment’s prior written consent; or (i iiv) use the Software for
// any illegal purpose.
// (b) Licensee may not distribute the DNTC in any manner.
// 
// LI C E N S E G R A N T .
// This license allows companies of any size, government entities or individuals to cre -
// ate, sell, rent, lease, or otherwise profit commercially from, games using executables
// created from the source code of DNT
// 
// **********************************************************************************
// **********************************************************************************
// **********************************************************************************
// THE SOURCE CODE GENERATED BY DNTC CAN BE  DISTRIBUTED PUBLICLY PROVIDED THAT THE 
// DISTRIBUTOR PROVIDES  THE GENERATE SOURCE CODE FREE OF CHARGE.
// 
// THIS SOURCE CODE (DNT) CAN BE DISTRIBUTED PUBLICLY PROVIDED THAT THE DISTRIBUTOR 
// PROVIDES  THE SOURCE CODE (DNT) FREE OF CHARGE.
// **********************************************************************************
// **********************************************************************************
// **********************************************************************************
// 
// Please visit http://www.winterleafentertainment.com for more information about the project and latest updates.
// 
// 
// 

#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using WinterLeaf.Classes;
using WinterLeaf.Containers;
using WinterLeaf.Enums;
using WinterLeaf.tsObjects;

#endregion

namespace DNT_FPS_Demo_Game_Dll.Tools
{
    public partial class Main : TorqueScriptTemplate
    {
        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "show", "", 1, 2500, false)]
        public void AddFMODProjectDlgShow(coGuiControl thisObj)
        {
            string fmodex;
            string fmodevent;
            if (sGlobal["$platform"].Equals("macos"))
            {
                fmodex = "libfmodex.dylib";
                fmodevent = "libfmodevent.dylib";
            }
            else
            {
                fmodex = "fmodex.dll";
                fmodevent = "fmod_event.dll";
            }

            // Make sure we have FMOD running.

            if(!Util.sfxGetDeviceInfo().Split('\t')[iGlobal["$SFX::DEVICE_INFO_PROVIDER"]].Equals("FMOD"))
            {
                console.Call("MessageBoxOK", new string[] {"Error",
                    "You do not currently have FMOD selected as your sound system.\n" +
                    "\n" +
                    "To install FMOD, place the FMOD DLLs (" + fmodex + " and " + fmodevent + ") " +
                    "in your game/ folder alongside your game executable " +
                    "and restart Torque.\n" +
                    "\n" +
                    "To select FMOD as your sound system, choose it as the sound provider in " +
                    "the audio tab of the Game Options dialog."});
                return;
            }

            // Make sure we have the FMOD Event DLL loaded.

            int deviceCaps = Util.sfxGetDeviceInfo().Split('\t')[iGlobal["$SFX::DEVICE_INFO_CAPS"]].AsInt();
            if ((deviceCaps & iGlobal["$SFX::DEVICE_CAPS_FMODDESIGNER"]) == 0)
            {
                console.Call("MessageBoxOK", new string[] { "Error",
                 "You do not have the requisite FMOD Event DLL in place.\n" +
                 "\n" +
                 "Please copy " + fmodevent + " into your game/ folder and restart Torque."});
                return;
            }

            // Show it.
            ((coGuiCanvas)console.GetObjectID("Canvas")).pushDialog(thisObj, "0", "true");
        }

        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "onWake", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnWake(coGuiControl thisObj)
        {
            thisObj["persistenceMgr"] = new Torque_Class_Helper("PersistenceManager").Create().AsString();
        }

        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "onSleep", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnSleep(coGuiControl thisObj)
        {
            ((coPersistenceManager)thisObj["persistenceMgr"]).delete();
        }

        [Torque_Decorations.TorqueCallBack("", "ConvexEditorGui", "onCancel", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnCancel(coGuiControl thisObj)
        {
            ((coGuiCanvas)console.GetObjectID("Canvas")).popDialog(thisObj);
        }

        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "onOK", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnOK(coGuiControl thisObj)
        {
            string objName = ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("projectNameField", true)).getText();
            string fileName = ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("fileNameField", true)).getText();
            string mediaPath = ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("mediaPathField", true)).getText();

            // Make sure the object name is valid.
            coEditManager EditManager = console.GetObjectID("EditManager");
            if (!console.Call("EditManagerValidateObjectName", objName, "true").AsBool())
                return;

            // Make sure the .fev file exists.
            if (fileName.Equals(""))
            {
                console.Call("MessageBoxOK", new string[] { "Error", "Please enter a project file name." });
                return;
            }
            if (!Util.isFile(fileName))
            {
                console.Call("MessageBoxOK", new string[] {"Error", "'" + fileName + "' is not a valid file."} );
                return;
            }

            // Make sure the media path exists.

            if (!Util.IsDirectory(mediaPath))
            {
                console.Call("MessageBoxOK", new string[] { "Error", "'" + mediaPath + "' is not a valid directory." });
                return;
            }

            // If an event script exists from a previous instantiation,
            // delete it first.

            string eventFileName = fileName + ".cs";
            if (Util.isFile(eventFileName))
                Util.fileDelete(eventFileName);

            // Create the FMOD project object.

            Util._pushInstantGroup();
            console.Eval("new SFXFMODProject( " + objName + ") {\n" +
                  "fileName = \"" + fileName + "\";\n" +
                  "mediaPath = \"" + mediaPath + "\";\n" +
               "};");
            Util._popInstantGroup();

            if (!console.isObject(objName))
            {
                console.Call("MessageBoxOK", new string[] { "Error", "Failed to create the object.  Please take a look at the log for details." });
                return;
            }
            else
            {
                // Save the object.

                ((coSimObject)objName).setFilename("scripts/client/audioData.cs");
                ((coPersistenceManager)thisObj["persistenceMgr"]).setDirty(objName);
                ((coPersistenceManager)thisObj["persistenceMgr"]).saveDirty();
            }

            ((coGuiCanvas)console.GetObjectID("Canvas")).popDialog(thisObj);

            // Trigger a reinit on the datablock editor, just in case.

            coScriptObject DatablockEditorPlugin = console.GetObjectID("DatablockEditorPlugin");
            if (DatablockEditorPlugin.isObject())
                DatablockEditorPlugin.call("populateTrees");
        }

        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "onSelectFile", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnSelectFile(coGuiControl thisObj)
        {
            string lastPath = sGlobal["$pref::WorldEditor::AddFMODProjectDlg::lastPath"];
            if (lastPath.Equals(""))
                lastPath = sGlobal["$pref::WorldEditor::AddFMODProjectDlg::lastPath"] = Util.getMainDotCsDir();

            coOpenFileDialog dlg;
            Torque_Class_Helper TCH = new Torque_Class_Helper("OpenFileDialog");
            TCH.Props.Add("Title", "Select Compiled FMOD Designer Event File...");
            TCH.Props.Add("Filters", "Compiled Event Files (*.fev)|*.fev|All Files (*.*)|*.*|");
            TCH.Props.Add("DefaultPath", lastPath);
            TCH.Props.Add("DefaultFile", Util.fileName(((coGuiTextEditCtrl)thisObj.findObjectByInternalName("fileNameField", true)).getText()));
            TCH.Props.Add("MustExit", "true");
            TCH.Props.Add("ChangePath", "false");
            dlg = TCH.Create();

            bool ret = dlg.Execute();
            string file = "";
            if (ret)
            {
                file = dlg.fileName;
                sGlobal["$pref::WorldEditor::AddFMODProjectDlg::lastPath"] = Util.filePath(file);
            }
            
            dlg.delete();

            if (!ret)
                return;

            file = Util.makeRelativePath(file, Util.getMainDotCsDir());
            ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("fileNameField", true)).setText(file);

            if (((coGuiTextEditCtrl)thisObj.findObjectByInternalName("projectNameField", true)).getText().Equals(""))
            {
                string projectName = "fmod" + Util.fileBase(file);
                if (Util._isValidObjectName(projectName))
                    ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("projectNameField", true)).setText(projectName);
            }
        }

        [Torque_Decorations.TorqueCallBack("", "AddFMODProjectDlg", "onSelectMediaPath", "", 1, 2500, false)]
        public void AddFMODProjectDlgOnSelectMediaPath(coGuiControl thisObj)
        {
            string defaultPath = ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("mediaPathField", true)).getText();
            if (defaultPath.Equals(""))
            {
                defaultPath = Util.filePath(((coGuiTextEditCtrl)thisObj.findObjectByInternalName("fileNameField", true)).getText());
                if (defaultPath.Equals(""))
                    defaultPath = Util.getMainDotCsDir();
                else
                    defaultPath = Util.makeFullPath(defaultPath,"");
            }
            coOpenFileDialog dlg;
            Torque_Class_Helper TCH = new Torque_Class_Helper("OpenFileDialog");
            TCH.Props.Add("Title", "Select Media Path...");
            TCH.Props.Add("DefaultPath", defaultPath);
            TCH.Props.Add("MustExit", "true");
            TCH.Props.Add("ChangePath", "false");
            dlg = TCH.Create();

            bool ret = dlg.Execute();
            string file = "";
            if (ret)
                file = dlg.fileName;

            dlg.delete();

            if (!ret)
                return;

            file = Util.makeRelativePath(file, Util.getMainDotCsDir());
            ((coGuiTextEditCtrl)thisObj.findObjectByInternalName("mediaPathField", true)).setText(file);
        }
    }
}
