using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
 
public class Board : MonoBehaviour
{

    public Vector3 red_angle, blue_angle, green_angle, yellow_angle;
    public Vector3 redh_up, redh_down, blueh_up, blueh_down, greenh_up, greenh_down, yellowh_up, yellowh_down;
    public Vector3 predh_up, predh_down, pblueh_up, pblueh_down, pgreenh_up, pgreenh_down, pyellowh_up, pyellowh_down;
    public ColorModes colorselected;
    homeManager[] homes;
    public homeManager red, blue, green, yellow;
    public bool active=false;

    void Awake()
    {

        homes = GetComponentsInChildren<homeManager>();
        green = homes.ToList().Find((h) => (h.homecolor == ColorModes.green));
        red = homes.ToList().Find((h) => (h.homecolor == ColorModes.red));
        blue = homes.ToList().Find((h) => (h.homecolor == ColorModes.blue));
        yellow = homes.ToList().Find((h) => (h.homecolor == ColorModes.yellow));

    }
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
           ArrengeBoard();
        }
        
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            foreach (homeManager h in homes)
            {
                h.DestroyTokens();
                h.DestroyPanel();
            }
        }
    }
    public void ArrengeBoard()
    {

        if (colorselected == ColorModes.green)
        {
            transform.rotation = Quaternion.Euler(green_angle);
            if (green)
                green.transform.rotation = Quaternion.Euler(greenh_down);
            // green.diceRoller.transform.rotation = Quaternion.Euler(pgreenh_down);
            if (yellow)
                yellow.transform.rotation = Quaternion.Euler(yellowh_down);
            // yellow.diceRoller.transform.rotation = Quaternion.Euler(pyellowh_down);

            if (red)
                red.transform.rotation = Quaternion.Euler(redh_up);
            // red.diceRoller.transform.rotation = Quaternion.Euler(predh_up);
            if (blue)
                blue.transform.rotation = Quaternion.Euler(blueh_up);
            // blue.diceRoller.transform.rotation = Quaternion.Euler(pblueh_up);

        }
        else
        if (colorselected == ColorModes.red)
        {
            transform.rotation = Quaternion.Euler(red_angle);
            if (green)
                green.transform.rotation = Quaternion.Euler(greenh_down);
            // green.diceRoller.transform.rotation = Quaternion.Euler(pgreenh_down);
            if (yellow)
                yellow.transform.rotation = Quaternion.Euler(yellowh_up);
            // yellow.diceRoller.transform.rotation = Quaternion.Euler(pyellowh_up);

            if (red)
                red.transform.rotation = Quaternion.Euler(redh_down);
            // red.diceRoller.transform.rotation = Quaternion.Euler(predh_down);
            if (blue)
                blue.transform.rotation = Quaternion.Euler(blueh_up);
            // blue.diceRoller.transform.rotation = Quaternion.Euler(pblueh_up);

        }
        else
        if (colorselected == ColorModes.blue)
        {
            transform.rotation = Quaternion.Euler(blue_angle);

            if (green)
                green.transform.rotation = Quaternion.Euler(greenh_up);
            // green.diceRoller.transform.rotation = Quaternion.Euler(pgreenh_up);

            if (yellow)
                yellow.transform.rotation = Quaternion.Euler(yellowh_up);
            // yellow.diceRoller.transform.rotation = Quaternion.Euler(pyellowh_up);


            if (red)
                red.transform.rotation = Quaternion.Euler(redh_down);
            // red.diceRoller.transform.rotation = Quaternion.Euler(predh_down);

            if (blue)
                blue.transform.rotation = Quaternion.Euler(blueh_down);
            // blue.diceRoller.transform.rotation = Quaternion.Euler(pblueh_down);

        }
        else
        if (colorselected == ColorModes.yellow)
        {
            transform.rotation = Quaternion.Euler(yellow_angle);

            if (green)
                green.transform.rotation = Quaternion.Euler(greenh_up);
            // green.diceRoller.transform.rotation = Quaternion.Euler(pgreenh_up);
            if (yellow)
                yellow.transform.rotation = Quaternion.Euler(yellowh_down);
            // yellow.diceRoller.transform.rotation = Quaternion.Euler(pyellowh_down);

            if (red)
                red.transform.rotation = Quaternion.Euler(redh_up);
            // red.diceRoller.transform.rotation = Quaternion.Euler(predh_up);
            if (blue)
            {

                blue.transform.rotation = Quaternion.Euler(blueh_down);
                // blue.diceRoller.transform.rotation = Quaternion.Euler(pblueh_down);
            }
        }
        SpawnTokensForeach();
        SpawnPlayPanel();
    }
    private void SpawnTokensForeach()
    {
        foreach (homeManager h in homes)
        {
            h.SpawnTokens();
        }
    }
    public void SpawnPlayPanel()
    {
        homes.ToList().ForEach((h) =>
        {
            h.SpawnPanel();
            h.uiArea.userpanel.ArrangePanel();
            h.SetDiceOnClick();
        });
        UpdateDetails();
        active=true;
    }
    public void UpdateDetails(){
        homes.ToList().ForEach((h) =>
        {
            h.uiArea.userpanel.SetPicture(h.uiArea.profileImage);
            h.uiArea.userpanel.SetUsername(h.uiArea.username);
        });
    }
}
