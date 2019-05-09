
namespace MediaServices.Demo.Function.Models
{
    public class ReservedUnits
    {
        public string odatametadata { get; set; }
        public Value[] value { get; set; }
    }

    public class Value
    {
        public string AccountId { get; set; }
        public int ReservedUnitType { get; set; }
        public int MaxReservableUnits { get; set; }
        public int CurrentReservedUnits { get; set; }
    }

}
