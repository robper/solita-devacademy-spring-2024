export async function load({params}){
    console.log(params);
    //let response = await fetch(`http://localhost:5221/stations/${params.station}`);
    //let station = await response.json() as Station;
    // Behöver inte lista alla depatures, bara antalet xd, så lägg till en endpoint på det, sen gör mer
    // Kan göra /count för dist & dur på depatrues & returns
    // Eller lägga den på /station endpoint
	let journey_resp = await fetch(`http://localhost:5221/stations/${params.station}/depatures`);
    console.log(journey_resp);
    let journeys = await journey_resp.json() as Journey[];

	return {
	//		station: station,
			journeys: journeys
	};
}