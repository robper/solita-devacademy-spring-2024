interface Station {
    id: number 
    station_name: string | null;
    station_address: string | null;
    coordinate_x: string | null;
    coordinate_y: string | null;
}
interface Journey {
    id: number 
    departure_date_time: string | null;
    return_date_time: string | null;
    departure_station_id: number;
    return_station_id: number;
    distance: number 
    duration: number 
}
interface Returnstation {
    return_station: number;
    count: number;
}