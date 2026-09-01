import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { api } from "../api/client";
import { useAuth } from "../context/AuthContext";

export default function ArtistsPage() {
    const { user } = useAuth();
    const [artists, setArtists] = useState([]);

    useEffect(() => { api.getArtists().then(setArtists).catch(() => { }); }, []);

    return (
        <div>
            <div className="d-flex justify-content-between align-items-center mb-4">
                <h2>Исполнители</h2>
                {user && <Link to="/artists/create" className="btn btn-outline-primary">+ Новый исполнитель</Link>}
            </div>

            {artists.length === 0 && <p className="text-muted">Исполнителей пока нет.</p>}

            <div className="row row-cols-1 row-cols-md-3 g-3">
                {artists.map((artist) => (
                    <div className="col" key={artist.id}>
                        <div className="card h-100">
                            <div className="card-body">
                                <h5 className="card-title">
                                    <Link to={`/artists/${artist.id}`} className="text-decoration-none">{artist.name}</Link>
                                </h5>
                                <p className="card-text text-muted small">{artist.songCount} песен</p>
                            </div>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}