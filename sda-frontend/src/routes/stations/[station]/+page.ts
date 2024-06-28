import { env } from '$env/dynamic/public';

export async function load({ params }) {
    console.log(params);
    console.log('Backend api url: ', env.PUBLIC_BACKEND_API);
    let station = await fetch(`${env.PUBLIC_BACKEND_API}/stations/${params.station}`);
    let dep_count = await fetch(`${env.PUBLIC_BACKEND_API}/stations/${params.station}/depatures/count`);
    let dep_dist = await fetch(`${env.PUBLIC_BACKEND_API}/stations/${params.station}/depatures/distance`);
    let dep_dur = await fetch(`${env.PUBLIC_BACKEND_API}/stations/${params.station}/depatures/duration`);
    let ret_count = await fetch(`${env.PUBLIC_BACKEND_API}/stations/${params.station}/returns/count`);
    return {
        station: await station.json() as Station,
        depatures_count: (await dep_count.json()).count as number,
        depatures_distance: (await dep_dist.json()).avg as number,
        depatures_duration: (await dep_dur.json()).avg as number,
        returns_count: (await ret_count.json()).count as number
    };
}