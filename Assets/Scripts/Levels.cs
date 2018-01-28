using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Levels{


	public static Level[] Stages = new Level[]{
		new Level(){
			LevelId = 1,
			LevelTitle = "Level 1",
			LevelTransmission = "I am you from the future"			
		},
		new Level(){
			LevelId = 2,
			LevelTitle = "Level 2",
			LevelTransmission = "With an important message"			
		},
		new Level(){
			LevelId = 3,
			LevelTitle = "Level 3",
			LevelTransmission = "About the past"			
		}
	};
}
public class Level {
	public int LevelId;
	public string LevelTitle;
	public string LevelTransmission;
}