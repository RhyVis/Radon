import { fileURLToPath, URL } from "node:url";

import { defineConfig } from "vite";
import { VitePWA } from "vite-plugin-pwa";
import { TDesignResolver } from "unplugin-vue-components/resolvers";
import vue from "@vitejs/plugin-vue";
import vueJsx from "@vitejs/plugin-vue-jsx";
import Components from "unplugin-vue-components/vite";
import AutoImport from "unplugin-auto-import/vite";
import Icon from "unplugin-icons/vite";
import IconsResolver from "unplugin-icons/resolver";

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    vueJsx(),
    VitePWA({
      injectRegister: "auto",
      registerType: "autoUpdate",
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
    }),
    Components({
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
