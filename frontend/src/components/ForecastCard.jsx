function formatDayLabel(dateStr) {
  const d = new Date(dateStr);
  if (Number.isNaN(d.getTime())) return dateStr;
  return d.toLocaleDateString(undefined, { weekday: "long", month: "long", day: "numeric" });
}

function formatShortDayLabel(dateStr) {
  const d = new Date(dateStr);
  if (Number.isNaN(d.getTime())) return dateStr;
  return d.toLocaleDateString(undefined, { weekday: "short", month: "short", day: "numeric" });
}

function summarizeDay(periods = []) {
  const temps = periods.map(p => p.temperature).filter(t => typeof t === "number");
  const high = temps.length ? Math.max(...temps) : null;
  const low = temps.length ? Math.min(...temps) : null;

  const day = periods.find(p => p.isDaytime === true) || periods[0];
  const summary = day?.shortForecast || "";
  const unit = day?.temperatureUnit || (periods[0]?.temperatureUnit ?? "F");

  // Choose “current” temperature: prefer first period temperature
  const current = typeof periods[0]?.temperature === "number" ? periods[0].temperature : high;

  return { high, low, summary, unit, current };
}

function pickConditionEmoji(text = "") {
  const t = text.toLowerCase();
  if (t.includes("thunder")) return "⛈️";
  if (t.includes("snow")) return "❄️";
  if (t.includes("rain") || t.includes("showers")) return "🌧️";
  if (t.includes("cloud")) return "☁️";
  if (t.includes("sun")) return "☀️";
  if (t.includes("clear")) return "🌙";
  return "🌤️";
}

export default function ForecastCard({ zoneName, data, loading }) {
  if (loading) {
    return (
      <div className="card">
        <div className="skeleton line" />
        <div className="skeleton block" />
      </div>
    );
  }

  if (!data || !Array.isArray(data) || data.length === 0) {
    return (
      <div className="card">
        <div className="muted">Pick a county zone to see forecast.</div>
      </div>
    );
  }

  // Today = first element
  const today = data[0];
  const todayPeriods = today?.currentDateData || [];
  const todaySummary = summarizeDay(todayPeriods);

  // “Current period” (first period of today)
  const nowPeriod = todayPeriods[0] || {};
  const nowLabel = nowPeriod?.name || "Now";
  const nowCondition = nowPeriod?.shortForecast || todaySummary.summary || "";
  const nowEmoji = pickConditionEmoji(nowCondition);

  // Remaining days as small cards
  const smallDays = data.slice(1, 7).map((d) => {
    const periods = d.currentDateData || [];
    const s = summarizeDay(periods);
    return {
      key: d.date,
      label: formatShortDayLabel(d.date),
      high: s.high,
      low: s.low,
      unit: s.unit,
      summary: s.summary,
    };
  });

  // Near-term list
  const nearTerm = data
    .slice(0, 2)
    .flatMap((d) => (d.currentDateData || []).map((p) => ({ ...p, _date: d.date })))
    .slice(0, 6);

  return (
    <div className="card">
      <div className="cardHeader">
        <div>
          <div className="cardTitle">{zoneName || "Forecast"}</div>
          <div className="muted small">Today + 7-day outlook</div>
        </div>
      </div>

      <div className="divider" />

      {/* hero card - today */}
      <div className="heroCard">
        <div className="heroLeft">
          <div className="heroIcon" aria-hidden>{nowEmoji}</div>
          <div className="heroTempRow">
            <div className="heroTemp">{todaySummary.current ?? "—"}</div>
            <div className="heroUnit">°{todaySummary.unit}</div>
          </div>

          <div className="heroMeta">
            <div className="heroMetaLine">
              <span className="heroMetaLabel">Hi</span>
              <span className="heroMetaValue">{todaySummary.high ?? "—"}°</span>
              <span className="heroMetaLabel">Lo</span>
              <span className="heroMetaValue">{todaySummary.low ?? "—"}°</span>
            </div>
            <div className="heroMetaDesc">{nowCondition}</div>
          </div>
        </div>

        <div className="heroRight">
          <div className="heroRightTitle">Weather</div>
          <div className="heroRightTime">
            {formatDayLabel(today.date)} • {nowLabel}
          </div>
          <div className="heroRightCond">{nowCondition}</div>
        </div>
      </div>

      <div className="divider" />
      
      {/* near term cards */}
      <div className="sectionTitleRow">
        <h3 className="sectionTitle">Near-term</h3>
      </div>

      <div className="hourlyList">
        {nearTerm.map((h, idx) => (
          <div key={`${h._date}-${idx}`} className="hourRow">
            <div className="hourTime">{h.name}</div>
            <div className="hourTemp">
              {h.temperature ?? "—"}°{h.temperatureUnit ?? ""}
            </div>
            <div className="hourDesc">{h.shortForecast ?? ""}</div>
          </div>
        ))}
      </div>

      <div className="divider" />

      {/* week cards */}
      <div className="sectionTitleRow">
        <h3 className="sectionTitle">Next 6 Days</h3>
      </div>

      <div className="dailyGrid smallGrid">
        {smallDays.map((d) => (
          <div key={d.key} className="miniCard">
            <div className="miniTitle">{d.label}</div>
            <div className="miniTemps">
              <span className="hi">Hi {d.high ?? "—"}°{d.unit}</span>
              <span className="lo">Lo {d.low ?? "—"}°{d.unit}</span>
            </div>
            <div className="miniMuted">{d.summary}</div>
          </div>
        ))}
      </div>

      <div className="divider" />

     
    </div>
  );
}