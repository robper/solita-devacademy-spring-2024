import { env } from '$env/dynamic/public';

export async function load({}){
    let response = await fetch(`${env.PUBLIC_BACKEND_API}/stations`);
    let stations = await response.json() as Station[];

	return {
			stations: stations
	};
}