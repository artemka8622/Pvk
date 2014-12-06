using System.Collections.Generic;
using System.Linq;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PVK.Control
{

    internal class DockPanelManagerHelper
    {
        private readonly List<RelationPair> _relations;

        private class RelationPair
        {
            public DockPanel DockPanel;
            public BarCheckItem BarCheckItem;
        }

        public DockPanelManagerHelper()
        {
            _relations = new List<RelationPair>();
        }

        public void AddRelation(DockPanel dockPanel, BarCheckItem barCheckItem)
        {
            dockPanel.VisibilityChanged += DockPanelVisibilityChanged;
            barCheckItem.ItemClick += BarCheckItemItemClick;
            _relations.Add(new RelationPair {BarCheckItem = barCheckItem,DockPanel = dockPanel});
            CheckDockPanelVisibility(dockPanel, barCheckItem);
        }

        private void BarCheckItemItemClick(object sender, ItemClickEventArgs e)
        {
            var barChaeckItem = (BarCheckItem) e.Item;
            var relationPair = _relations.FirstOrDefault(r => r.BarCheckItem == barChaeckItem);
            if (relationPair != null)
                SetDockPanelVisibility(barChaeckItem, relationPair.DockPanel);
        }

        private void DockPanelVisibilityChanged(object sender, VisibilityChangedEventArgs e)
        {
            var dockPanel = (DockPanel) sender;
            var relationPair = _relations.FirstOrDefault(r => r.DockPanel == dockPanel);
            if (relationPair != null)
                CheckDockPanelVisibility(dockPanel, relationPair.BarCheckItem);
        }

        private static void SetDockPanelVisibility(BarCheckItem barCheckItem, DockPanel dockPanel)
        {
            if (barCheckItem.Checked)
            {
                //if (MainForm.CheckOperation(dockPanel))
                //{
					if (dockPanel.Visibility == DockVisibility.Hidden) 
						dockPanel.Visibility = DockVisibility.Visible;
                //}
                //else
                //    barCheckItem.Checked = false;
            }
            else
            {
                dockPanel.Visibility = DockVisibility.Hidden;
            }
        }

        private static void CheckDockPanelVisibility(DockPanel dockPanel, BarCheckItem barCheckItem)
        {
            barCheckItem.Checked = dockPanel.Visibility != DockVisibility.Hidden;
        }
    }
}