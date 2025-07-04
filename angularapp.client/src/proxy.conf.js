const PROXY_CONFIG = [
  {
    context: ["/api"],
    target: "http://localhost:5068",
    secure: false,
    changeOrigin: true
  }
];

module.exports = PROXY_CONFIG;
