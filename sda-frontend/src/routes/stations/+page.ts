export async function load({}){
    let response = await fetch("http://localhost:5221/stations");
    let stations = await response.json() as Station[];

	return {
			stations: stations
	};
}