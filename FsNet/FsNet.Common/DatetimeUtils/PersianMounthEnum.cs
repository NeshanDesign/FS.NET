namespace FsNet.Common.DatetimeUtils
{
    public enum PersianMounth
    {
        Farvardin = 1,
        Ordibehesht = 2,
        Khordad = 3,
        Tir = 4,
        Mordad = 5,
        Sharhrivar = 6,
        Mehr = 7,
        Aban = 8,
        Azar = 9,
        Dey = 10,
        Bahman = 11,
        Esfand = 12
    }

    public static class PersianMounthExtentions
    {
        public static string ToPersianString(this PersianMounth mounth)
        {
            switch (mounth)
            {
                case PersianMounth.Farvardin: return Properties.Resources.Farvardin;
                case PersianMounth.Ordibehesht: return Properties.Resources.Ordibehesht;
                case PersianMounth.Khordad: return Properties.Resources.Khordad;
                case PersianMounth.Tir: return Properties.Resources.Tir;
                case PersianMounth.Mordad: return Properties.Resources.Mordad;
                case PersianMounth.Sharhrivar: return Properties.Resources.Shahrivar;
                case PersianMounth.Mehr: return Properties.Resources.Mehr;
                case PersianMounth.Aban: return Properties.Resources.Aban;
                case PersianMounth.Azar: return Properties.Resources.Azar;
                case PersianMounth.Dey: return Properties.Resources.Dey;
                case PersianMounth.Bahman: return Properties.Resources.Bahman;
                case PersianMounth.Esfand: return Properties.Resources.Esfand;
            }
            return null;
        }
    }
}
