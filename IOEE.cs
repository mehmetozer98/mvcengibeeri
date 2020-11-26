namespace MVCEngiBeering.Interface
{
    interface IOEE
    {
        void AddTime(double shiftLengthInHours, double breakLengthInHours);

        double GetOEE();

        double GetGoodCount();

        double GetPlannedProductionTime();

        double GetIdealCycleTime();
    }
}