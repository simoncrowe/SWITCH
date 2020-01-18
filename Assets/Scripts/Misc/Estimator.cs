using UnityEngine;
using System.Collections;

public class Estimator : MonoBehaviour {
	
	int length;
	int maxLength;
	int minChoices;
	int maxChoices;
	int totalNodes;
	int counter;
	int prevNodes;
	
	IEnumerator Start () {
		length = 20;
		minChoices = 2;
		maxChoices = 5;
		prevNodes = 1;
		totalNodes = 1;

		//executes steps
		for(int curStep=0; curStep<length; curStep++){
			counter = 0;
			//executes node creation based on most recent total of previous nodes
			for(int n=0; n<prevNodes; n++){
				//creates nodes
				int numDests = Random.Range(minChoices,maxChoices);
				for (int i=0; i<numDests; i++){
				counter ++;
				totalNodes ++;
				}
			}
			yield return null;					
			prevNodes = counter;
	}
			print ("Estimated total nodes for diologue of "+length+" steps in length:"+totalNodes);

	}
}