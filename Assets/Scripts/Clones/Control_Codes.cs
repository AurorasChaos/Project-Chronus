using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_Codes 
{
    List<Control_Code> control_Codes = new List<Control_Code>();

    public void SetupControlCodes()
    {
        control_Codes.Add(new Control_Code(101, "StartOfPackets", "This signifies the starting point of a players playback recording." ));
        control_Codes.Add(new Control_Code(102, "Normal", "This signifies that there's nothing strange about the packet."));
        control_Codes.Add(new Control_Code(103, "SkipPacket-NoChange", "This signifies that we can skip this packet safely due to no change from last."));
        //More to be added inbetween these.
        control_Codes.Add(new Control_Code(109, "EndOfPackets", "This signifies the ending point of a players playback recording."));
        control_Codes.Add(new Control_Code(110, "PlayerDiedEarly", "Kinda Obvious"));
        control_Codes.Add(new Control_Code(999, "Undefined Error", "We should probably look into this."));

    }

    public int GetControlCodeByName(string name)
    {
        try { return control_Codes.Find(x => x.GetName() == name).GetID(); }
            
        catch { return 999; }
            
    }


}

public class Control_Code
{
    int ID;
    string Name;
    string Description;

    public Control_Code(int _ID, string _Name, string _Description)
    {
        ID = _ID;
        Name = _Name;
        Description = _Description;
    }

    public int GetID()
    {
        return ID;
    }
    public string GetName()
    {
        return Name;
    }
    public string GetDescription()
    {
        return Description;
    }
}
