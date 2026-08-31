import { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import { api } from "../api/client";
import { useAuth } from "../context/AuthContext";

export default function SongDetailsPage() {
  const { id } = useParams();
  const { user } = useAuth();
  const [song, setSong] = useState(null);
  const [comments, setComments] = useState(null);
  const [commentText, setCommentText] = useState("");
  const [error, setError] = useState("");

  const loadSong = () => api.getSong(id).then(setSong).catch((e) => setError(e.message));
  const loadComments = (page = 1) => api.getComments(id, page).then(setComments).catch(() => {});

  useEffect(() => {
    loadSong();
    loadComments();
  }, [id]);

  const submitComment = async (e) => {
    e.preventDefault();
    if (!commentText.trim()) return;
    await api.addComment(id, commentText.trim());
    setCommentText("");
    loadComments();
  };

  const rate = async (value) => {
    await api.rateSong(id, value);
    loadSong();
  };

  if (error) return <div className="alert alert-danger">{error}</div>;
  if (!song) return <p>Загрузка...</p>;

  return (
    <div className="row">
      <div className="col-md-8">
        <h2>{song.title}</h2>
        <h5 className="text-muted">
          <Link to={`/artists/${song.artistId}`}>{song.artistName}</Link>
        </h5>

        <p>
          {song.genres.map((g) => (
            <span key={g.id} className="badge bg-secondary genre-badge me-1">{g.name}</span>
          ))}
        </p>

        <p className="text-muted">
          ⭐ {song.rating.toFixed(1)} &nbsp;|&nbsp; ▶ {song.playCount} прослушиваний
        </p>

        <audio controls src={api.playUrl(song.id)} className="w-100 mb-3" />

        <div className="d-flex gap-2 mb-4">
          {user && (
            <a className="btn btn-success" href={api.downloadUrl(song.id)}>Скачать</a>
          )}
        </div>

        {user ? (
          <div className="card mb-4">
            <div className="card-body">
              <h6 className="card-title">Ваша оценка</h6>
              <div className="d-flex gap-2">
                {[1, 2, 3, 4, 5].map((i) => (
                  <button key={i} className="btn btn-outline-warning btn-sm" onClick={() => rate(i)}>{i}</button>
                ))}
              </div>
            </div>
          </div>
        ) : (
          <div className="alert alert-light border">
            <Link to="/login">Войдите</Link>, чтобы оценить песню и оставить комментарий.
          </div>
        )}

        <h5>Комментарии</h5>

        {user && (
          <form onSubmit={submitComment} className="mb-3">
            <textarea
              className="form-control mb-2"
              rows={2}
              maxLength={1000}
              placeholder="Ваш комментарий..."
              value={commentText}
              onChange={(e) => setCommentText(e.target.value)}
              required
            />
            <button type="submit" className="btn btn-sm btn-primary">Отправить</button>
          </form>
        )}

        {comments?.items?.length ? (
          comments.items.map((c) => (
            <div className="border-bottom py-2" key={c.id}>
              <strong>{c.username}</strong>{" "}
              <span className="text-muted small">{new Date(c.createdAt).toLocaleString("ru-RU")}</span>
              <p className="mb-0">{c.text}</p>
            </div>
          ))
        ) : (
          <p className="text-muted">Комментариев пока нет.</p>
        )}

        {comments?.totalPages > 1 && (
          <nav className="mt-3">
            <ul className="pagination pagination-sm">
              {Array.from({ length: comments.totalPages }, (_, i) => i + 1).map((p) => (
                <li key={p} className={`page-item ${p === comments.page ? "active" : ""}`}>
                  <button className="page-link" onClick={() => loadComments(p)}>{p}</button>
                </li>
              ))}
            </ul>
          </nav>
        )}
      </div>
    </div>
  );
}
