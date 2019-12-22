using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feature : MonoBehaviour
{
    // Rule
    public int infecteeID;

    public static int MAX_HEIGHT = 180;
    public static int MAX_WEIGHT = 100;

    public static float MAX_RECOGNIZE_RANGE = 20f;

    // Feature Data
    public enum eye_color { RED, DARK_RED };
    public enum damagedBody_degree { NORMAL, HARD };
    public enum damagedBrain_degree { NORMAL, HARD };
    public enum damagedCloth_degree { NORMAL, HARD };
    public enum walking_power { NORMAL, GOOD };
    public enum running_power { NORMAL, GOOD };
    public enum attack_tendency { NORMAL, STRONG };
    public enum persist_degree { NORMAL, STRONG };

    public eye_color myEyeColor;
    public damagedBody_degree myBody;
    public damagedBrain_degree myBrain;
    public damagedCloth_degree myCloth;
    public walking_power wkPower;
    public running_power rnPower;
    public attack_tendency akTendency;
    public persist_degree psDgr;

    public float height;
    public float weight;
    public float senseOfSight;
    public float senseOfSmell;
    public float senseOfHearing;
    public float senseOfTouch;
    public float senseOfTaste;
    public float RecognizeRange;

    public Transform[] Ankles;

    void OnEnable()
	{
		GetComponent<Health>().hp = 100;
		GenerateFeatures();
	}

    public void GenerateFeatures()
    {
        switch (infecteeID)
        {
            case 0:
                myEyeColor = eye_color.RED;
                myBody = damagedBody_degree.NORMAL;
                myBrain = damagedBrain_degree.NORMAL;
                myCloth = damagedCloth_degree.NORMAL;
                wkPower = walking_power.NORMAL;
                rnPower = running_power.GOOD;
                akTendency = attack_tendency.NORMAL;
                psDgr = persist_degree.NORMAL;

                height = 180;
                weight = 50;
                senseOfSight = 0.5f; ;
                senseOfSmell = 0.3f;
                senseOfHearing = 0.8f;
                senseOfTouch = 1f;
                senseOfTaste = 0.2f;
                RecognizeRange = 15f;
                break;

            case 1:
                myEyeColor = eye_color.RED;
                myBody = damagedBody_degree.NORMAL;
                myBrain = damagedBrain_degree.NORMAL;
                myCloth = damagedCloth_degree.NORMAL;
                wkPower = walking_power.GOOD;
                rnPower = running_power.NORMAL;
                akTendency = attack_tendency.STRONG;
                psDgr = persist_degree.STRONG;

                height = 160;
                weight = 100;
                senseOfSight = 0.9f; ;
                senseOfSmell = 0.7f;
                senseOfHearing = 0.2f;
                senseOfTouch = 0.5f;
                senseOfTaste = 0.8f;
                RecognizeRange = 10f;
                break;

            case 2:
                myEyeColor = eye_color.RED;
                myBody = damagedBody_degree.HARD;
                myBrain = damagedBrain_degree.HARD;
                myCloth = damagedCloth_degree.HARD;
                wkPower = walking_power.NORMAL;
                rnPower = running_power.GOOD;
                akTendency = attack_tendency.NORMAL;
                psDgr = persist_degree.STRONG;

                height = 160;
                weight = 100;
                senseOfSight = 0.5f;
                senseOfSmell = 0.7f;
                senseOfHearing = 0.8f;
                senseOfTouch = 0.5f;
                senseOfTaste = 0.8f;
                RecognizeRange = 10f;
                break;

            case 3:
                myEyeColor = eye_color.DARK_RED;
                myBody = damagedBody_degree.NORMAL;
                myBrain = damagedBrain_degree.HARD;
                myCloth = damagedCloth_degree.HARD;
                wkPower = walking_power.NORMAL;
                rnPower = running_power.NORMAL;
                akTendency = attack_tendency.STRONG;
                psDgr = persist_degree.NORMAL;

                height = 180;
                weight = 100;
                senseOfSight = 0.9f;
                senseOfSmell = 0.3f;
                senseOfHearing = 0.2f;
                senseOfTouch = 1f;
                senseOfTaste = 0.8f;
                RecognizeRange = 10f;
                break;

            case 4:
                myEyeColor = eye_color.DARK_RED;
                myBody = damagedBody_degree.HARD;
                myBrain = damagedBrain_degree.NORMAL;
                myCloth = damagedCloth_degree.HARD;
                wkPower = walking_power.GOOD;
                rnPower = running_power.GOOD;
                akTendency = attack_tendency.STRONG;
                psDgr = persist_degree.NORMAL;

                height = 160;
                weight = 50;
                senseOfSight = 0.5f;
                senseOfSmell = 0.3f;
                senseOfHearing = 0.2f;
                senseOfTouch = 0.5f;
                senseOfTaste = 0.2f;
                RecognizeRange = 15f;
                break;

            case 5:
                myEyeColor = eye_color.DARK_RED;
                myBody = damagedBody_degree.HARD;
                myBrain = damagedBrain_degree.HARD;
                myCloth = damagedCloth_degree.NORMAL;
                wkPower = walking_power.GOOD;
                rnPower = running_power.NORMAL;
                akTendency = attack_tendency.NORMAL;
                psDgr = persist_degree.STRONG;

                height = 180;
                weight = 50;
                senseOfSight = 0.9f;
                senseOfSmell = 0.7f;
                senseOfHearing = 0.8f;
                senseOfTouch = 1f;
                senseOfTaste = 0.2f;
                RecognizeRange = 15f;
                break;

        }
    }
}
