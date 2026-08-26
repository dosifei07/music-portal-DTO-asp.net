window.addEventListener("load", function () {
    const btn = document.createElement("a");
    btn.href = "https://localhost:7195/admin-spa.html";
    btn.innerText = "Назад в SPA";
    btn.style.cssText = "position:fixed;top:10px;right:20px;z-index:9999;background:#0d6efd;color:#fff;padding:8px 14px;border-radius:4px;text-decoration:none;font-family:sans-serif;";
    document.body.appendChild(btn);
});