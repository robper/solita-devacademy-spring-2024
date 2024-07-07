import { env } from '$env/dynamic/public';
// Needed for leaflet https://github.com/Leaflet/Leaflet/issues/6552
// (window.onload) might work too but we prefere sveltes onMount

export const ssr = false; 

export async function load({}){
    let response = await fetch(`${env.PUBLIC_BACKEND_API}/stations`);
    let stations = await response.json() as Station[];

	return {
			stations: stations
	};
}