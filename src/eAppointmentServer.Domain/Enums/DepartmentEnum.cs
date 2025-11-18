using System;
using Ardalis.SmartEnum;

namespace eAppointmentServer.Domain.Enums;

public sealed class DepartmentEnum : SmartEnum<DepartmentEnum>
{
    public static readonly DepartmentEnum Acil = new("Acil", 1);
    public static readonly DepartmentEnum Cocuk = new("Cocuk", 2);
    public static readonly DepartmentEnum Dahiliye = new("Dahiliye", 3);
    public static readonly DepartmentEnum Dis = new("Dis", 4);
    public static readonly DepartmentEnum Goz = new("Goz", 5);
    public static readonly DepartmentEnum KBB = new("KBB", 6);
    public static readonly DepartmentEnum Ortopedi = new("Ortopedi", 7);
    public static readonly DepartmentEnum Psikiyatri = new("Psikiyatri", 8);
    public static readonly DepartmentEnum Nefroloji = new("Nefroloji", 9);
    public static readonly DepartmentEnum Noroloji = new("Noroloji", 10);
    public static readonly DepartmentEnum GenelCerrahi = new("Genel Cerrahi", 11);
    public static readonly DepartmentEnum Endrokrinoloji = new("Endrokrinoloji", 12);
    public static readonly DepartmentEnum Dermatoloji = new("Dermatoloji", 13);
    public static readonly DepartmentEnum Kardioloji = new("Kardioloji", 14);
    public DepartmentEnum(string name, int value) : base(name, value)
    {
    }
}
