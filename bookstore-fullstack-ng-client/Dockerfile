# =========================
# 1. Base Node image
# =========================
FROM node:20 AS build
WORKDIR /app

# Copy package.json and package-lock.json first for caching
COPY package*.json ./

# Install dependencies
RUN npm install -g @angular/cli \
    && npm install

# Copy the rest of the Angular app
COPY . .

# =========================
# 2. Development Mode
# =========================
# This will run ng serve directly
EXPOSE 4200
CMD ["ng", "serve", "--host", "0.0.0.0", "--port", "4200"]
