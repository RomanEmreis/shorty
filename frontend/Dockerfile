FROM node:latest
WORKDIR /app
ENV services__shorty-api__http__0=http://localhost:8080
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 3000
CMD [ "npm", "start" ]
