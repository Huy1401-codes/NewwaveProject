import React, { useState } from "react";
import authApi from "../api/authApi";

const LoginPage = () => {
  const [emailOrPhone, setEmailOrPhone] = useState("");
  const [password, setPassword] = useState("");

  const [error, setError] = useState("");
  const [user, setUser] = useState(null);

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const data = await authApi.login({ emailOrPhone, password });

      localStorage.setItem("accessToken", data.accessToken);
      localStorage.setItem("refreshToken", data.refreshToken);

      setUser(data.user);
      alert("Đăng nhập thành công!");

    } catch (err) {
      setError("Đăng nhập thất bại. Kiểm tra lại thông tin!");
      console.error(err);
    }
  };

  return (
    <div style={{ maxWidth: 400, margin: "50px auto" }}>
      <h2>Đăng nhập</h2>

      <form onSubmit={handleLogin}>
        <input
          type="text"
          placeholder="Email hoặc Số điện thoại"
          value={emailOrPhone}
          onChange={(e) => setEmailOrPhone(e.target.value)}
          required
          style={{ width: "100%", padding: 8, marginBottom: 10 }}
        />

        <input
          type="password"
          placeholder="Mật khẩu"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={{ width: "100%", padding: 8, marginBottom: 10 }}
        />

        <button
          type="submit"
          style={{
            width: "100%",
            padding: 10,
            background: "black",
            color: "white",
            border: "none",
            cursor: "pointer",
          }}
        >
          Đăng nhập
        </button>
      </form>

      {error && <p style={{ color: "red", marginTop: 10 }}>{error}</p>}

      {user && (
        <div style={{ marginTop: 20, padding: 10, background: "#eee" }}>
          <h3>Đăng nhập thành công!</h3>
          <p><b>Họ tên:</b> {user.fullName}</p>
          <p><b>Email:</b> {user.email}</p>
          <p><b>Vai trò:</b> {user.role}</p>
        </div>
      )}
    </div>
  );
};

export default LoginPage;
