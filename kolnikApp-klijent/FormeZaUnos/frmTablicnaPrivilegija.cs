﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using kolnikApp_komponente;

namespace kolnikApp_klijent.FormeZaUnos
{
    public partial class frmTablicnaPrivilegija : Form
    {
        public frmTablicnaPrivilegija()
        {
            InitializeComponent();
            radno_mjestoComboBox.DataSource = (from radno_mjestoObj in DataHandler.entityNamesWithReferencesToBelongingDataStores["radno_mjesto"]
                                               select ((radno_mjesto)radno_mjestoObj).naziv).ToArray();
            radno_mjestoComboBox.SelectedIndex = -1;           
        }

        private void GumbIzlaz_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GumbReset_Click(object sender, EventArgs e)
        {
            radno_mjestoComboBox.SelectedIndex = -1;
            naziv_tabliceComboBox.SelectedIndex = -1;
            UpozorenjeNazivTablice.Hide();
            UpozorenjeRadnoMjesto.Hide();
            UpozorenjeCheckbox.Hide();
            for(int i=0; i < operacijeCheckedListBox.Items.Count; i++)
            {
                operacijeCheckedListBox.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private bool oznacenaBarJednaOpcija()
        {
            bool OznacenaOpcija = false;
            if(operacijeCheckedListBox.CheckedItems.Count > 0)
            {
                OznacenaOpcija = true;
                UpozorenjeCheckbox.Hide();
            }
            else
            {
                UpozorenjeCheckbox.Show();
            }
            return OznacenaOpcija;
        }

        private void popuniLabeleUpozorenja(Label LabelaUpozorenja)
        {
            string TekstUpozorenjaComboBox = "Odaberite element";
            LabelaUpozorenja.Text = TekstUpozorenjaComboBox;
            LabelaUpozorenja.Show();
        }

        private void GumbPotvrda_Click(object sender, EventArgs e)
        {
            if (radno_mjestoComboBox.SelectedIndex == -1)
            {
                popuniLabeleUpozorenja(UpozorenjeRadnoMjesto);
            }
            if (naziv_tabliceComboBox.SelectedIndex == -1)
            {
                popuniLabeleUpozorenja(UpozorenjeNazivTablice);
            }
            if (!oznacenaBarJednaOpcija())
            {
                UpozorenjeCheckbox.Show();
            }
            if(radno_mjestoComboBox.SelectedIndex != -1 && naziv_tabliceComboBox.SelectedIndex != -1 && oznacenaBarJednaOpcija())
            {
                //pohrani podatke u klasu i pošalji u BP
                this.Close();
            }
            
        }

        private string KapitalizirajString(string [] PojamZaIzmjenu)
        {
            string uljepsanoIme = "";
            for (int i = 0; i < PojamZaIzmjenu.Length; i++)
            {
                uljepsanoIme += PojamZaIzmjenu[i].First().ToString().ToUpper() + PojamZaIzmjenu[i].Substring(1);
                if (i + 1 != PojamZaIzmjenu.Length)
                {
                    uljepsanoIme += " ";
                }
            }
            return uljepsanoIme;
        }

        
        private void radno_mjestoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> PopisTablica = new List<string>();
            UpozorenjeRadnoMjesto.Hide();
            if (radno_mjestoComboBox.SelectedIndex != -1)
            {     
                string[] tablice = DataHandler.entityNamesWithReferencesToBelongingDataStores.Keys.ToArray<string>();
                string[] ImaPravo = (from tablicna_privilegijaObj in DataHandler.entityNamesWithReferencesToBelongingDataStores["tablicna_privilegija"]
                                     from radno_mjestoObj in DataHandler.entityNamesWithReferencesToBelongingDataStores["radno_mjesto"]
                                     where ((tablicna_privilegija)tablicna_privilegijaObj).radno_mjesto == ((radno_mjesto)radno_mjestoObj).id &&
                                           ((radno_mjesto)radno_mjestoObj).naziv == radno_mjestoComboBox.SelectedValue.ToString()
                                     select ((tablicna_privilegija)tablicna_privilegijaObj).naziv_tablice).ToArray();

                IEnumerable<string> RazlikaSviIRadnik = tablice.Except(ImaPravo);
                foreach (var NazivTablice in RazlikaSviIRadnik)
                {
                    string[] ime = NazivTablice.Split('_');
                    PopisTablica.Add(KapitalizirajString(ime));
                }
                naziv_tabliceComboBox.DataSource = PopisTablica;
                naziv_tabliceComboBox.SelectedIndex = -1;
            }
        }

        private void naziv_tabliceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpozorenjeNazivTablice.Hide();
        }

        private void operacijeCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            UpozorenjeCheckbox.Hide();
        }
    }
}
