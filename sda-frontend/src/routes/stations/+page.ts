import { PUBLIC_BACKEND_API } from '$env/static/public';

export async function load({}){
    let response = await fetch(`${PUBLIC_BACKEND_API}/stations`);
    let stations = await response.json() as Station[];

	return {
			stations: stations
	};
}