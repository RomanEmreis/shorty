const { createProxyMiddleware } = require("http-proxy-middleware");
module.exports = function (app) {
  const apiUrl            = process.env["services__ingress__https__0"];
  const isProduction      = process.env["NODE_ENV"] !== "development";
  const tokenRe           = /\/[a-zA-Z0-9]{5}$/;

  const createTokenProxy  = createProxyMiddleware({
    target:       apiUrl,
    secure:       isProduction,
    pathRewrite:  { "^/api": "/" },
    changeOrigin: true
  });

  const resolveTokenProxy = createProxyMiddleware({
    target:       apiUrl,
    secure:       isProduction,
    pathRewrite:  { "^/": "/" },
    pathFilter:   (path, req) => path.match(tokenRe) && req.method === 'GET',
    changeOrigin: true
  });

  app.use('/api', createTokenProxy);
  app.use('/', resolveTokenProxy);
};