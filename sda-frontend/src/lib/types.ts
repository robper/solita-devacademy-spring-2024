interface Station {
    id: number
    station_name: string | undefined;
    station_address: string | undefined;
    coordinate_x: string | undefined;
    coordinate_y: string | undefined;
}
interface Journey {
    id: number
    departure_date_time: string | undefined;
    return_date_time: string | undefined;
    departure_station_id: number;
    return_station_id: number;
    distance: number
    duration: number
}
interface Returnstation {
    return_station: number;
    count: number;
}
interface SingleStationData {
    station: Station;
    depatures_count: number;
    depatures_distance: number;
    depatures_duration: number;
    returns_count: number;
}