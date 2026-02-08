import { useEffect, useMemo, useState } from "react";
import Calendar from "react-calendar";
import { Dialog,DialogPanel } from "@headlessui/react";
import "react-calendar/dist/Calendar.css";
import "./calendar-overrides.css";

const API_BASE = import.meta.env.VITE_API_BASE ?? "https://localhost:7213";

// =====================
// Helpers
// =====================
const toYmd = (date) => {
  const d = new Date(date);
  return d.toISOString().split("T")[0];
};

const diffDays = (a, b) =>
  (new Date(b) - new Date(a)) / (1000 * 60 * 60 * 24);

// Dias sem fluxo entre ciclos
const daysBetween = (end, nextStart) => {
  const diff =
    (new Date(nextStart) - new Date(end)) /
    (1000 * 60 * 60 * 24);

  return diff > 0 ? diff - 1 : 0;
};

function groupByCycles(ciclos) {
  if (!ciclos || ciclos.length === 0) return [];

  const sorted = [...ciclos].sort(
    (a, b) => new Date(a.data) - new Date(b.data)
  );

  const cycles = [];
  let current = {
    start: new Date(sorted[0].data),
    end: new Date(sorted[0].data),
    days: [sorted[0]],
  };

  for (let i = 1; i < sorted.length; i++) {
    const prev = sorted[i - 1];
    const curr = sorted[i];

    if (diffDays(prev.data, curr.data) === 1) {
      current.end = new Date(curr.data);
      current.days.push(curr);
    } else {
      cycles.push(current);
      current = {
        start: new Date(curr.data),
        end: new Date(curr.data),
        days: [curr],
      };
    }
  }

  cycles.push(current);
  return cycles.reverse(); 
}

export default function App() {
  const [ciclos, setCiclos] = useState([]);
  const [calendarValue, setCalendarValue] = useState(new Date());

  const [open, setOpen] = useState(false);
  const [selectedDate, setSelectedDate] = useState(new Date());
  const [editing, setEditing] = useState(null);
  const [fluxo, setFluxo] = useState("Leve");
  const [loading, setLoading] = useState(false);

  const load = async () => {
    const res = await fetch(`${API_BASE}/api/ciclos`);
    setCiclos(await res.json());
  };

  useEffect(() => {
    load();
  }, []);

  const ciclosByDay = useMemo(() => {
    const map = new Map();
    ciclos.forEach((c) => map.set(toYmd(c.data), c));
    return map;
  }, [ciclos]);

  const cycles = useMemo(() => groupByCycles(ciclos), [ciclos]);

  const cyclesWithInterval = useMemo(() => {
    return cycles.map((cycle, index) => {
      const next = cycles[index - 1];
      return {
        ...cycle,
        gapToNext: next
          ? daysBetween(cycle.end, next.start)
          : null,
      };
    });
  }, [cycles]);

  const prediction = useMemo(() => {
    const gaps = cyclesWithInterval
      .map(c => c.gapToNext)
      .filter(g => g !== null && g > 0);

    if (gaps.length === 0) return null;

    const avgGap =
      gaps.reduce((a, b) => a + b, 0) / gaps.length;

    const lastCycle = cyclesWithInterval[0];
    const predictedStart = new Date(lastCycle.end);
    predictedStart.setDate(
      predictedStart.getDate() + Math.round(avgGap) + 1
    );

    return {
      averageGap: Math.round(avgGap),
      predictedStart,
    };
  }, [cyclesWithInterval]);

  const [openCycle, setOpenCycle] = useState(null);

  const openModalForDay = (date) => {
    const key = toYmd(date);
    setSelectedDate(date);
    setEditing(ciclosByDay.get(key) ?? null);
    setFluxo(ciclosByDay.get(key)?.fluxo ?? "Leve");
    setOpen(true);
  };

  const submit = async () => {
    setLoading(true);

    const payload = {
      data: toYmd(selectedDate),
      fluxo,
    };

    if (!editing) {
      await fetch(`${API_BASE}/api/ciclos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });
    } else {
      await fetch(`${API_BASE}/api/ciclos/${editing.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(payload),
      });
    }

    await load();
    setLoading(false);
    setOpen(false);
  };

  const removeDay = async (id) => {
    const ok = confirm("Deseja deletar este registro?");
    if (!ok) return;

    await fetch(`${API_BASE}/api/ciclos/${id}`, {
      method: "DELETE",
    });

    await load();
  };

  const tileClassName = ({ date, view }) => {
    if (view !== "month") return "";
    return ciclosByDay.has(toYmd(date)) ? "mc-day-marked" : "";
  };


  return (
    <div className="min-h-screen bg-pink-100 p-6">
      <div className="mx-auto max-w-5xl space-y-5">

        <div className="flex justify-between items-end">
          <div>
            <h1 className="text-3xl font-extrabold text-pink-700">
              MeuCiclo
            </h1>
            <p className="text-sm text-gray-600">
              Clique em um dia para registrar, editar ou deletar.
            </p>
          </div>

          <button
            onClick={() => openModalForDay(new Date())}
            className="rounded-xl bg-pink-600 px-4 py-2 text-white font-semibold"
          >
            + Registrar hoje
          </button>
        </div>

        {prediction && (
          <div className="rounded-2xl bg-white p-4 shadow border-l-4 border-pink-500">
            <div className="font-bold text-pink-700">
              Previsão do próximo ciclo
            </div>
            <div className="text-sm text-gray-700">
              Média de intervalo:{" "}
              <b>{prediction.averageGap} dias</b>
            </div>
            <div className="text-sm mt-1">
              Próximo ciclo estimado para:
              <b className="ml-1 text-pink-600">
                {prediction.predictedStart.toLocaleDateString(
                  "pt-BR"
                )}
              </b>
            </div>
          </div>
        )}

        <div className="grid gap-5 lg:grid-cols-[420px_1fr]">

          <div className="bg-white rounded-2xl p-4 shadow">
            <Calendar
              locale="pt-BR"
              value={calendarValue}
              onChange={setCalendarValue}
              onClickDay={openModalForDay}
              tileClassName={tileClassName}
            />
          </div>


          <div className="space-y-3 max-h-[70vh] overflow-y-auto pr-2">
            {cyclesWithInterval.map((cycle, idx) => {
              const isOpen = openCycle === idx;

              return (
                <div
                  key={idx}
                  className="rounded-2xl border bg-white shadow"
                >
                  <button
                    onClick={() =>
                      setOpenCycle(isOpen ? null : idx)
                    }
                    className="w-full flex justify-between items-center p-4 hover:bg-pink-50 rounded-2xl transition"
                  >
                    <div className="text-left">
                      <div className="font-bold">
                        {cycle.start.toLocaleDateString("pt-BR")}{" "}
                        {cycle.start.getTime() !==
                          cycle.end.getTime() &&
                          `→ ${cycle.end.toLocaleDateString(
                            "pt-BR"
                          )}`}
                      </div>

                      <div className="text-xs space-y-1 text-gray-500">
                        <div>{cycle.days.length} dia(s)</div>

                        {cycle.gapToNext !== null ? (
                          <div className="text-pink-600 font-medium">
                            Intervalo até o próximo ciclo:{" "}
                            {cycle.gapToNext} dia(s)
                          </div>
                        ) : (
                          <div className="italic text-gray-400">
                            Último ciclo registrado
                          </div>
                        )}
                      </div>
                    </div>

                    <span
                      className={`text-pink-600 font-semibold transition-transform duration-300 ${
                        isOpen ? "rotate-180" : ""
                      }`}
                    >
                      ▼
                    </span>
                  </button>

                  <div
                    className={`
                      overflow-hidden transition-all duration-300
                      ${isOpen ? "max-h-96 opacity-100" : "max-h-0 opacity-0"}
                    `}
                  >
                    <div className="border-t bg-pink-50 px-4 py-3 space-y-2">
                      {cycle.days.map((d) => (
                        <div
                          key={d.id}
                          className="flex items-center justify-between bg-white rounded-xl px-3 py-2"
                        >
                          <div>
                            <div className="text-sm font-medium">
                              {new Date(d.data).toLocaleDateString(
                                "pt-BR"
                              )}
                            </div>
                            <div className="text-xs text-gray-500">
                              Fluxo: {d.fluxo}
                            </div>
                          </div>

                          <button
                            onClick={() => removeDay(d.id)}
                            className="text-red-600 text-sm font-semibold hover:text-red-800"
                          >
                            Deletar
                          </button>
                        </div>
                      ))}
                    </div>
                  </div>
                </div>
              );
            })}
          </div>
        </div>


        <Dialog open={open} onClose={() => setOpen(false)}>
          <div className="fixed inset-0 bg-black/40 flex items-center justify-center">
            <DialogPanel className="bg-white rounded-2xl p-5 w-full max-w-md">
              <DialogPanel className="font-bold text-lg">
                {editing ? "Editar registro" : "Novo registro"}
              </DialogPanel>

              <p className="text-sm text-gray-600 mb-3">
                {selectedDate.toLocaleDateString("pt-BR")}
              </p>

              <select
                className="w-full border rounded-xl px-3 py-2 mb-4"
                value={fluxo}
                onChange={(e) => setFluxo(e.target.value)}
              >
                <option>Leve</option>
                <option>Moderado</option>
                <option>Intenso</option>
              </select>

              <div className="flex gap-2">
                <button
                  onClick={submit}
                  disabled={loading}
                  className="flex-1 bg-pink-600 text-white rounded-xl py-2"
                >
                  Salvar
                </button>

                <button
                  onClick={() => setOpen(false)}
                  className="bg-gray-100 rounded-xl px-4"
                >
                  Cancelar
                </button>
              </div>
            </DialogPanel>
          </div>
        </Dialog>
      </div>
    </div>
  );
}
