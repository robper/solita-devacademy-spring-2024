import { env } from '$env/dynamic/public';

export async function load({ }) {
	let response = await fetch(`${env.PUBLIC_BACKEND_API}/stations`);
	let stations = await response.json() as Station[];
	return {
		stations: stations.sort((a, b) => {
			if (!a.station_name || !b.station_name) {
				return 0;
			}
			if (a.station_name < b.station_name) {
				return -1;
			}
			if (a.station_name > b.station_name) {
				return 1;
			}
			return 0;
		})
	};
}