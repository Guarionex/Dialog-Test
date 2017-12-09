using System.Collections;
using System.Collections.Generic;

public interface INPCAction
{
    void setActive();
    void talk();
    void setDeactive();
    string npcName();
    void idle();
}
