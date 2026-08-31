import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../api/client";

export default function SongUploadPage() {
  const navigate = useNavigate();
  const [title, setTitle] = useState("");
  const [file, setFile] = useState(null);
  const [artistId, setArtistId] = useState("");
  const [artists, setArtists] = useState([]);
  const [genres, setGenres] = useState([]);
  const [genreIds, setGenreIds] = useState([]);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    api.getArtistsBrief().then(setArtists).catch(() => {});
    api.getGenres().then(setGenres).catch(() => {});
  }, []);

  const toggleGenre = (id) => {
    setGenreIds((prev) => (prev.includes(id) ? prev.filter((g) => g !== id) : [...prev, id]));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors({});

    const formData = new FormData();
    formData.append("Title", title);
    formData.append("ArtistId", artistId);
    if (file) formData.append("File", file);
    genreIds.forEach((id) => formData.append("GenreIds", id));

    try {
      const songId = await api.uploadSong(formData);
      navigate(`/songs/${songId}`);
    } catch (err) {
      try {
        const parsed = JSON.parse(err.message);
        setErrors(parsed.errors || { _: [err.message] });
      } catch {
        setErrors({ _: [err.message] });
      }
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <h2 className="mb-4">Загрузить песню</h2>
        {errors._ && <div className="alert alert-danger">{errors._.join(", ")}</div>}
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label">Название</label>
            <input className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} required />
          </div>

          <div className="mb-3">
            <label className="form-label">Исполнитель</label>
            <select className="form-select" value={artistId} onChange={(e) => setArtistId(e.target.value)} required>
              <option value="">— выберите исполнителя —</option>
              {artists.map((a) => (
                <option key={a.id} value={a.id}>{a.name}</option>
              ))}
            </select>
          </div>

          <div className="mb-3">
            <label className="form-label">Аудиофайл (mp3, wav, flac, ogg)</label>
            <input
              type="file"
              className="form-control"
              accept=".mp3,.wav,.flac,.ogg"
              onChange={(e) => setFile(e.target.files[0])}
              required
            />
            <div className="form-text">Максимум 50 МБ.</div>
          </div>

          <div className="mb-3">
            <label className="form-label">Жанры</label>
            {genres.map((g) => (
              <div className="form-check" key={g.id}>
                <input
                  type="checkbox"
                  className="form-check-input"
                  id={`genre-${g.id}`}
                  checked={genreIds.includes(g.id)}
                  onChange={() => toggleGenre(g.id)}
                />
                <label className="form-check-label" htmlFor={`genre-${g.id}`}>{g.name}</label>
              </div>
            ))}
          </div>

          <button type="submit" className="btn btn-primary">Загрузить</button>
        </form>
      </div>
    </div>
  );
}
