const { createProxyMiddleware } = require("http-proxy-middleware");
module.exports = function (app) {
  const apiUrl            = process.env["services__ingress__https__0"];
  const isProduction      = process.env["NODE_ENV"] !== "development";
  const tokenRe           = /\/[a-zA-Z0-9]{7}$/;

  const createTokenProxy  = createProxyMiddleware({
    target:       apiUrl,
    secure:       isProduction,
    pathRewrite:  { "^/": "/create" },
    method:       "POST",
    changeOrigin: true,
    logger: console
  });

  const resolveTokenProxy = createProxyMiddleware({
    target:       apiUrl,
    secure:       isProduction,
    pathRewrite:  { "^/": "/" },
    method:       "GET",
    pathFilter:   (path, _) => path.match(tokenRe),
    changeOrigin: true
  });

  app.use('/create', createTokenProxy);
  app.use('/'      , resolveTokenProxy);
};