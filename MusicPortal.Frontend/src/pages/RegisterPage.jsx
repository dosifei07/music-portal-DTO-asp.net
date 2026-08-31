import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { api } from "../api/client";

export default function RegisterPage() {
  const navigate = useNavigate();
  const [form, setForm] = useState({
    username: "", email: "", password: "", confirmPassword: "",
    isArtistRequested: false, artistName: "", bio: "",
  });
  const [errors, setErrors] = useState({});
  const [info, setInfo] = useState("");

  const update = (field) => (e) => {
    const value = e.target.type === "checkbox" ? e.target.checked : e.target.value;
    setForm((f) => ({ ...f, [field]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setErrors({});

    if (form.password !== form.confirmPassword) {
      setErrors({ confirmPassword: "Пароли не совпадают" });
      return;
    }

    try {
      const res = await api.register(form);
      if (res.isFirstUser) {
        setInfo(res.message);
        setTimeout(() => navigate("/login"), 2000);
      } else {
        navigate("/registration-pending");
      }
    } catch (err) {
      try {
        const parsed = JSON.parse(err.message);
        setErrors({ [parsed.field || "_"]: parsed.error });
      } catch {
        setErrors({ _: err.message });
      }
    }
  };

  return (
    <div className="row justify-content-center">
      <div className="col-md-6">
        <h2 className="mb-4">Регистрация</h2>
        {info && <div className="alert alert-success">{info}</div>}
        {errors._ && <div className="alert alert-danger">{errors._}</div>}
        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label">Имя пользователя</label>
            <input className="form-control" value={form.username} onChange={update("username")} required minLength={3} maxLength={100} />
            {errors.Username && <div className="text-danger">{errors.Username}</div>}
          </div>
          <div className="mb-3">
            <label className="form-label">Электронная почта</label>
            <input type="email" className="form-control" value={form.email} onChange={update("email")} required />
            {errors.Email && <div className="text-danger">{errors.Email}</div>}
          </div>
          <div className="mb-3">
            <label className="form-label">Пароль</label>
            <input type="password" className="form-control" value={form.password} onChange={update("password")} required minLength={6} />
          </div>
          <div className="mb-3">
            <label className="form-label">Подтверждение пароля</label>
            <input type="password" className="form-control" value={form.confirmPassword} onChange={update("confirmPassword")} required />
            {errors.confirmPassword && <div className="text-danger">{errors.confirmPassword}</div>}
          </div>

          <hr />

          <div className="form-check mb-3">
            <input type="checkbox" className="form-check-input" id="artistCheck" checked={form.isArtistRequested} onChange={update("isArtistRequested")} />
            <label className="form-check-label" htmlFor="artistCheck">Зарегистрироваться как Исполнитель (требует подтверждения)</label>
          </div>

          {form.isArtistRequested && (
            <>
              <div className="mb-3">
                <label className="form-label">Сценическое имя</label>
                <input className="form-control" value={form.artistName} onChange={update("artistName")} maxLength={100} />
              </div>
              <div className="mb-3">
                <label className="form-label">О себе / Биография</label>
                <textarea className="form-control" rows={3} value={form.bio} onChange={update("bio")} maxLength={1000} />
              </div>
            </>
          )}

          <div className="alert alert-info">
            После регистрации ваша заявка будет рассмотрена администратором портала.
          </div>

          <button type="submit" className="btn btn-primary w-100">Зарегистрироваться</button>
        </form>
        <p className="mt-3 text-center">
          Уже есть аккаунт? <Link to="/login">Войти</Link>
        </p>
      </div>
    </div>
  );
}
