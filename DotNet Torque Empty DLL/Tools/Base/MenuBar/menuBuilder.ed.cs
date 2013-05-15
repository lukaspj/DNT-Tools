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
        // Adds one item to the menu.
        // if %item is skipped or "", we will use %item[#], which was set when the menu was created.
        // if %item is provided, then we update %item[#].
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "addItem", "", 3, 2500, false)]
        public void MenuBuilderAddItem(coPopupMenu thisObj, int pos, string item)
        {
            if (item.Equals(""))
                item = thisObj["item[" + pos + "]"];
            if (!item.Equals(thisObj["item[" + pos + "]"]))
                thisObj["item[" + pos + "]"] = item;
            string[] itemSplit = item.Split('\t');
            string name = itemSplit[0];
            string accel = itemSplit[1];
            string cmd = itemSplit[2];

            // We replace the [this] token with our object ID
            cmd = cmd.Replace("[this]", thisObj);
            thisObj["item[" + pos + "]"] = name + "\t" + accel + "\t" + cmd;

            // If %accel is an object, we want to add a sub menu
            if (console.isObject(accel))
                thisObj.insertSubMenu(pos.AsString(), name, accel);
            else
                thisObj.insertItem(pos.AsString(), name != "-" ? name : "", accel);
        }
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "appendItem", "", 3, 2500, false)]
        public void MenuBuilderAppendItem(coPopupMenu thisObj, int pos, string item)
        {
            MenuBuilderAddItem(thisObj, thisObj.getItemCount(), item); 
        }
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "onAdd", "", 1, 2500, false)]
        public void MenuBuilderOnAdd(coPopupMenu thisObj)
        {
            if (!((coGuiCanvas)thisObj["canvas"]).isObject())
                thisObj["canvas"] = console.GetObjectID("Canvas").AsString();
            for (int i = 0; thisObj["item[" + i + "]"] != ""; i++)
                MenuBuilderAddItem(thisObj, i, "");
        }
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "onRemove", "", 1, 2500, false)]
        public void MenuBuilderOnRemove(coPopupMenu thisObj)
        {
            thisObj.removeFromMenuBar();
        }
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "onSelectItem", "", 3, 2500, false)]
        public bool MenuBuilderOnSelectItem(coPopupMenu thisObj, int id, string text)
        {
            string cmd = thisObj["item[" + id + "]"].Split('\t')[2];
            if (!cmd.Equals(""))
            {
                console.Eval(cmd);
                return true;
            }
            return false;
        }
        /// Sets a new name on an existing menu item.
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "setItemName", "", 3, 2500, false)]
        public void MenuBuilderSetItemName(coPopupMenu thisObj, int id, string name)
        {
            string item = thisObj["item["+id+"]"];
            string accel = item.Split('\t')[1];
            thisObj.setItem(id.AsString(), name, accel);
        }
        /// Sets a new command on an existing menu item.
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "setItemCommand", "", 3, 2500, false)]
        public void MenuBuilderSetItemCommand(coPopupMenu thisObj, int id, string command)
        {
            string[] itemSplit = thisObj["item["+id+"]"].Split('\t');
            thisObj["item[" + id + "]"] = itemSplit[0] + "\t" + itemSplit[1] + "\t" + command;
        }
        /// (SimID this)
        /// Wraps the attachToMenuBar call so that it does not require knowledge of
        /// barName or barIndex to be removed/attached.  This makes the individual 
        /// MenuBuilder items very easy to add and remove dynamically from a bar.
        ///
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "attachToMenuBar", "%this, nameSpaceDepth", 1, 2500, false)]
        public bool MenuBuilderAttachToMenuBar(coPopupMenu thisObj, int nameSpaceDepth)
        {
            if(thisObj["barName"].Equals(""))
            {
                console.error("MenuBuilder::attachToMenuBar - Menu property 'barName' not specified.");
                return false;
            }
            if(thisObj["barPosition"].AsInt() < 0)
            {
                console.error("MenuBuilder::attachToMenuBar - Menu  " + thisObj["barName"] + " property 'barPosition' is invalid, must be zero or greater.");
                return false;
            }
            console.ParentExecute(thisObj, "attachToMenuBar", nameSpaceDepth - 1, 
                new string[] { thisObj["canvas"], thisObj["barPosition"], thisObj["barName"] }
                );
            return true;
        }

        // Callbacks from PopupMenu. These callbacks are now passed on to submenus
        // in C++, which was previously not the case. Thus, no longer anything to
        // do in these. I am keeping the callbacks in case they are needed later.
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "onAttachToMenuBar", "", 4, 2500, false)]
        public void MenuBuilderOnAttachToMenuBar(coPopupMenu thisObj, coGuiCanvas canvas, int pos, string title)
        {
        }
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "onRemoveFromMenuBar", "", 2, 2500, false)]
        public void MenuBuilderOnRemoveFromMenuBar(coPopupMenu thisObj, coGuiCanvas canvas)
        {
        }

        /// Method called to setup default state for the menu. Expected to be overriden
        /// on an individual menu basis. See the mission editor for an example.
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "setupDefaultState", "", 1, 2500, false)]
        public void MenuBuilderSetupDefaultState(coPopupMenu thisObj)
        {
            for(int i = 0; !thisObj["item["+i+"]"].Equals(""); i++)
            {
                string[] itemSplit = thisObj["item["+i+"]"].Split('\t');
                string name = itemSplit[0];
                string accel = itemSplit[1];
                string cmd = itemSplit[2];

                // Pass on to sub menus
                if(console.isObject(accel))
                    MenuBuilderSetupDefaultState(accel);
            }
        }

        /// Method called to easily enable or disable all items in a menu.
        [Torque_Decorations.TorqueCallBack("", "MenuBuilder", "enableAllItems", "", 2, 2500, false)]
        public void MenuBuilderEnableAllItems(coPopupMenu thisObj, bool enable)
        {
            for (int i = 0; !thisObj["item[" + i + "]"].Equals(""); i++)
            {
                thisObj.enableItem(i.AsString(), enable.AsString());
            }
        }

    }
}
