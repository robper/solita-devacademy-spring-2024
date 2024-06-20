export async function load({}){
    let response = await fetch("http://localhost:5221/stations");
    let stations = await response.json() as Station[];
    
	let journey_resp = await fetch("http://localhost:5221/journeys");
    let journeys = await journey_resp.json() as Journey[];

	return {
			stations: stations,
			journeys: journeys
	};
}