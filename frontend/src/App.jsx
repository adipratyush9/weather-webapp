

import { useEffect, useMemo, useState } from "react";
import Header from "./components/Header";
import Select from "./components/Select";
import ForecastCard from "./components/ForecastCard";
import { api } from "./api/client";
import { STATES } from "./data/states";
import "./index.css";



   
      export default function App() {
        const [stateCode, setStateCode] = useState("");
        const [entities, setEntities] = useState(null);

        const [zoneId, setZoneId] = useState("");
        const [zoneForecast, setZoneForecast] = useState(null);

        const [loadingEntities, setLoadingEntities] = useState(false);
        const [loadingForecast, setLoadingForecast] = useState(false);
        const [error, setError] = useState("");

        const stateOptions = useMemo(
          () => STATES.map((s) => ({ value: s.code, label: `${s.name} (${s.code})` })),
          []
        );

        const zoneOptions = useMemo(() => {
          const zones = entities?.zones ?? entities?.Zones ?? []; // supports camelCase or PascalCase
          return zones.map((z) => ({
            value: z.id ?? z.Id,
            label: `${z.name ?? z.Name} (${z.id ?? z.Id})`,
          }));
        }, [entities]);

        const selectedZoneName = useMemo(() => {
          const zones = entities?.zones ?? entities?.Zones ?? [];
          const z = zones.find((x) => (x.id ?? x.Id) === zoneId);
          return z?.name ?? z?.Name ?? "";
        }, [entities, zoneId]);

        // Fetch entities on state change
        useEffect(() => {
          if (!stateCode) return;

          (async () => {
            setError("");
            setLoadingEntities(true);
            setEntities(null);
            setZoneId("");
            setZoneForecast(null);

            try {
              const res = await api.get(`/api/locations/state/${stateCode}`);
              setEntities(res.data?.data ?? res.data);
            } catch (e) {
              setError(e?.response?.data?.message || e?.message || "Failed to load entities");
            } finally {
              setLoadingEntities(false);
            }
          })();
        }, [stateCode]);

        // Fetch zone forecast on zone change
        useEffect(() => {
          if (!zoneId) return;

          (async () => {
            setError("");
            setLoadingForecast(true);
            setZoneForecast(null);

            try {
              const res = await api.get(`/api/forecast/zone/${zoneId}`);
              setZoneForecast(res.data?.data ?? res.data);
            } catch (e) {
              setError(e?.response?.data?.message || e?.message || "Failed to load forecast");
            } finally {
              setLoadingForecast(false);
            }
          })();
        }, [zoneId]);

        return (
          <div className="bg">
            <div className="overlay">
              <div className="container">
                <Header />

                <div className="panel">
                  <div className="grid">
                    <Select
                      label="State"
                      value={stateCode}
                      onChange={setStateCode}
                      options={stateOptions}
                      placeholder="Select a state"
                      disabled={false}
                    />

                    <Select
                      label="County Zones"
                      value={zoneId}
                      onChange={setZoneId}
                      options={zoneOptions}
                      placeholder={loadingEntities ? "Loading zones..." : "Select a county"}
                      disabled={!stateCode || loadingEntities || zoneOptions.length === 0}
                    />
                  </div>

                  <div className="statusRow">
                    {error && <div className="error">{error}</div>}
                    {!error && loadingEntities && <div className="muted">Loading entities…</div>}
                  </div>
                </div>

                <ForecastCard zoneName={selectedZoneName} data={zoneForecast} loading={loadingForecast} />
              </div>
            </div>
          </div>
        );
      }

 