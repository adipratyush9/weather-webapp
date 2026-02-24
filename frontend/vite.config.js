import { defineConfig, loadEnv } from "vite";
import react from "@vitejs/plugin-react";

export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), "");

  return {
    plugins: [react()],
    server: {
      port: 5173, // if taken, Vite automatically uses 5174, etc.
      proxy: {
        "/api": {
          target: env.VITE_BACKEND_URL || "http://localhost:5266",
          changeOrigin: true,
          secure: false,
        },
      },
    },
  };
});