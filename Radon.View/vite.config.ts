import { fileURLToPath, URL } from "node:url";

import VueI18nPlugin from "@intlify/unplugin-vue-i18n/vite";
import vue from "@vitejs/plugin-vue";
import vueJsx from "@vitejs/plugin-vue-jsx";
import AutoImport from "unplugin-auto-import/vite";
import IconsResolver from "unplugin-icons/resolver";
import Icon from "unplugin-icons/vite";
import { TDesignResolver } from "unplugin-vue-components/resolvers";
import Components from "unplugin-vue-components/vite";
import { defineConfig } from "vite";
import { VitePWA } from "vite-plugin-pwa";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueJsx(),
    VueI18nPlugin({
      include: fileURLToPath(new URL("./src/**/*.{yml,yaml,vue,json5}", import.meta.url)),
      defaultSFCLang: "yaml",
    }),
    VitePWA({
      injectRegister: "auto",
      registerType: "autoUpdate",
      includeAssets: [
        "/favicon.ico",
        "/android-chrome-192x192.png",
        "/android-chrome-512x512.png",
        "/apple-touch-icon.png",
      ],
      manifest: {
        name: "Radon",
        short_name: "Rn",
        description: "Simple Web App for personal use",
        start_url: "./",
        display: "standalone",
        theme_color: "#ffffff",
        icons: [
          {
            src: "/android-chrome-192x192.png",
            sizes: "192x192",
            type: "image/png",
          },
          {
            src: "/android-chrome-512x512.png",
            sizes: "512x512",
            type: "image/png",
          },
          {
            src: "/apple-touch-icon.png",
            sizes: "180x180",
            type: "image/png",
          },
        ],
      },
      workbox: {
        globPatterns: ["**/*.{html,wasm,js,css,ico,png,jpg,svg,ttf,otf,woff,woff2}"],
        navigateFallback: "/index.html",
        navigateFallbackDenylist: [/^\/api\//, /^\/dev_local/],
        runtimeCaching: [
          { urlPattern: /^\/api\//, handler: "NetworkOnly" },
          { urlPattern: /^\/dev_local/, handler: "NetworkOnly" },
          { urlPattern: "/index.html", handler: "NetworkOnly" },
          { urlPattern: "/", handler: "NetworkOnly" },
          {
            urlPattern: /\.(?:ttf|otf|woff|woff2)$/,
            handler: "CacheFirst",
            options: {
              cacheName: "font-cache",
              expiration: {
                maxEntries: 50,
                maxAgeSeconds: 30 * 24 * 60 * 60, // 30 days
              },
            },
          },
        ],
      },
    }),
    Components({
      dirs: ["src/components", "src/layout/frame"],
      resolvers: [
        IconsResolver(),
        TDesignResolver({
          library: "vue-next",
        }),
      ],
    }),
    AutoImport({
      resolvers: [
        TDesignResolver({
          library: "vue-next",
        }),
      ],
    }),
    Icon(),
  ],
  build: {
    chunkSizeWarningLimit: 850,
  },
  css: {
    preprocessorOptions: {
      less: {},
    },
  },
  resolve: {
    alias: {
      "@": fileURLToPath(new URL("./src", import.meta.url)),
    },
  },
  server: {
    proxy: {
      "/dev": {
        target: "http://localhost:5000",
        rewrite: path => path.replace(/^\/dev_local/, ""),
        changeOrigin: true,
      },
    },
  },
});
