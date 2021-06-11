using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static readonly string[] elements = {

        "Hydrogen", "Helium",
        "Lithium", "Berylium", "Boron", "Carbon", "Nitrogen", "Oxygen", "Fluorine", "Neon",
        "Sodium", "Magnesium", "Alluminum", "Silicon", "Phosphorus", "Sulfur", "Chlorine", "Argon",

        "Potassium", "Calcium", "Scandium", "Titanium", "Vanadium", "Chromium", "Manganese", "Iron", "Cobalt",
        "Nickel", "Copper", "Zinc", "Gallium", "Germanium", "Arsenic", "Selenium", "Bromine", "Krypton",

        "Rubidium", "Strontium", "Yttrium", "Zirconium", "Niobium", "Molybdenum", "Technetium", "Ruthenium",
        "Rhodium", "Palladium", "Silver", "Cadmium", "Indium", "Tin", "Antimony", "Tellurium", "Iodine", "Xenon"
    };

    //elements but arranged in the periodic table
    public static readonly string[,] periodicTable = new string [,] {
        { "Hydrogen", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "Helium", },

        { "Lithium", "Berylium", "", "", "", "", "", "", "", "", "", "", "Boron", "Carbon", "Nitrogen", "Oxygen", "Fluorine", "Neon", },
        { "Sodium", "Magnesium", "", "", "", "", "", "", "", "", "", "", "Alluminum", "Silicon", "Phosphorus", "Sulfur", "Chlorine", "Argon", },

        { "Potassium", "Calcium", "Scandium", "Titanium", "Vanadium", "Chromium", "Manganese", "Iron", "Cobalt",
        "Nickel", "Copper", "Zinc", "Gallium", "Germanium", "Arsenic", "Selenium", "Bromine", "Krypton", },

        { "Rubidium", "Strontium", "Yttrium", "Zirconium", "Niobium", "Molybdenum", "Technetium", "Ruthenium",
        "Rhodium", "Palladium", "Silver", "Cadmium", "Indium", "Tin", "Antimony", "Tellurium", "Iodine", "Xenon" }
    };

    public static readonly string[] symbols = {

        "H", "He",
        "Li", "Be", "B", "C", "N", "O", "F", "Ne",
        "Na", "Mg", "Al", "Si", "P", "S", "Cl", "Ar",

        "K", "Ca", "Sc", "Ti", "V", "Cr", "Mn", "Fe", "Co",
        "Ni", "Cu", "Zn", "Ga", "Ge", "As", "Se", "Br", "Kr",

        "Rb", "Sr", "Y", "Zr", "Nb", "Mo", "Tc", "Ru",
        "Rh", "Pd", "Ag", "Cd", "In", "Sn", "Sb", "Te", "I", "Xe"
    };

    public static readonly int[] atomRadii =
    {
        25, -1,
        145, 105, 85, 70, 65, 60, 50, -1,
        180, 150, 184, 210, 180, 180, 175, -1,
        220, 180, 160, 140, 135, 140, 140, 140, 135, 135, 135, 135, 130, 125, 115, 115, 115, -1,
        235, 200, 180, 155, 145, 145, 135, 130, 135, 140, 160, 155, 155, 145, 145, 140, 140, -1
    };

    public static readonly int[] atomicWeights = { 1, 4, 7, 9, 11, 12, 14, 16, 19, 20, 23, 24, 27, 28, 31, 32, 35, 40,
    39, 40, 45, 48, 51, 52, 55, 56, 59, 59, 64, 65, 70, 73, 75, 79, 80, 84, 85, 88, 89, 91, 93, 96, 98, 101,
    103, 106, 108, 112, 115, 119, 122, 128, 127, 131};

    public static readonly int[] ePerShell = { 0, 2, 8, 8, 18, 18 };

    public static readonly string[] type = {

        "Non-Metal", "Non-Metal",
        "Metal", "Metal", "Metaloid", "Non-Metal", "Non-Metal", "Non-Metal", "Non-Metal", "Non-Metal",
        "Metal", "Metal", "Metal", "Metalloid", "Non-Metal", "Non-Metal", "Non-Metal", "Non-Metal",

        "Metal", "Metal", "Metal", "Metal", "Metal", "Metal","Metal", "Metal", "Metal","Metal", "Metal", "Metal",
        "Metal", "Metalloid", "Metalloid", "Non-Metal", "Non-Metal", "Non-Metal",

        "Metal", "Metal", "Metal", "Metal", "Metal", "Metal","Metal", "Metal", "Metal","Metal", "Metal", "Metal",
        "Metal", "Metal", "Metalloid", "Metalloid", "Non-Metal", "Non-Metal"
    };  //Metal / nonMetal / Metalloid

    public static readonly string[] state = {

        "Gas", "Gas",
        "Solid", "Solid", "Solid", "Solid", "Gas", "Gas", "Gas", "Gas",
        "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Gas", "Gas",

        "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
        "Solid", "Solid", "Solid", "Solid", "Liquid", "Gas",

        "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid", "Solid",
        "Solid", "Solid", "Solid", "Solid", "Solid", "Gas"

    };

    public static (int period, int column) getElementPeriodAndColumn(string element)
    {
        for (int p = 0; p < periodicTable.GetLength(0); p++)
        {
            for (int col = 0; col < periodicTable.GetLength(1); col++)
            {
                if (periodicTable[p, col].Equals(element)) 
                    return (p+1, col+1);
            }
        }

        return (0, 0);
    }

    //Gets element in period above. if none, returns previous noble gas
    public static string getElementAbove(string element)    
    {
        string elementAbove = "";

        (int period, int column) = getElementPeriodAndColumn(element);

        if (period == 1)
        {
            elementAbove = "Hydrogen";
        } else
        {
            elementAbove = periodicTable[period-2, column-1];

            //find previous noble gas if no element above
            if (elementAbove.Equals(""))
                elementAbove = periodicTable[period - 2, 17];
        }

        return elementAbove;
    }

}
