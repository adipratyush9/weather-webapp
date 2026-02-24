export default function Select({ label, value, onChange, options, placeholder, disabled }) {
  return (
    <label className="field">
      <span className="label">{label}</span>
      <select
        className="select"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        disabled={disabled}
      >
        <option value="">{placeholder}</option>
        {options.map((o) => (
          <option key={o.value} value={o.value}>
            {o.label}
          </option>
        ))}
      </select>
    </label>
  );
}