namespace HospitalSystem
{
    public interface IInputValidator
    {
        int ValidAge((int minAge, int maxAge) ageRange);
        string ValidDate();
        string ValidEmail();
        int ValidFloor(int maxFloors = 6);
        string ValidMobile();
        string ValidName();
        string ValidNewPassword();
        string ValidPassword();
        int ValidRoomNumber();
        string ValidSpeciality();
        int ValidStaffId();
    }
}