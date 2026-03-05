using System;
using System.Collections.Generic;
using System.Linq;
using PR15.Models;

namespace PR15.Services
{
    public static class CompatibilityChecker
    {
        public static List<string> CheckCompatibility(List<basepart_> selectedParts)
        {
            var errors = new List<string>();
            using (var context = Core.Context)
            {
                // Типы частей: 1=CPU, 2=GPU, 3=RAM, 4=Motherboard, 5=Case, 6=PSU, 7=Cooler, 8=Storage
                var cpu = selectedParts.FirstOrDefault(p => p.parttypeid == 1);
                var motherboard = selectedParts.FirstOrDefault(p => p.parttypeid == 4);
                var ram = selectedParts.Where(p => p.parttypeid == 3).ToList();
                var gpu = selectedParts.Where(p => p.parttypeid == 2).ToList();
                var psu = selectedParts.FirstOrDefault(p => p.parttypeid == 6);
                var cooler = selectedParts.FirstOrDefault(p => p.parttypeid == 7);
                var casePart = selectedParts.FirstOrDefault(p => p.parttypeid == 5);

                // 1. Совместимость сокета (CPU и Материнская плата)
                if (cpu != null && motherboard != null)
                {
                    var cpuSocket = context.cpu_.FirstOrDefault(c => c.id == cpu.id)?.socketid;
                    var mbSocket = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.socketid;

                    if (cpuSocket.HasValue && mbSocket.HasValue && cpuSocket != mbSocket)
                        errors.Add("❌ Процессор и материнская плата имеют разные сокеты!");
                }

                // 2. Совместимость сокета (CPU и Кулер)
                if (cpu != null && cooler != null)
                {
                    var cpuSocket = context.cpu_.FirstOrDefault(c => c.id == cpu.id)?.socketid;
                    if (cpuSocket.HasValue)
                    {
                        var isCompatible = context.socketprocessorcooler_.Any(spc =>
                            spc.socketid == cpuSocket && spc.processorcoolerid == cooler.id);

                        if (!isCompatible)
                            errors.Add("❌ Кулер не совместим с сокетом процессора!");
                    }
                }

                // 3. Совместимость форм-фактора (Материнская плата и Корпус)
                if (motherboard != null && casePart != null)
                {
                    var mbFormFactor = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.formfactorid;
                    if (mbFormFactor.HasValue)
                    {
                        var isCompatible = context.boardformfactorcase_.Any(bfc =>
                            bfc.caseid == casePart.id && bfc.formfactorid == mbFormFactor);

                        if (!isCompatible)
                            errors.Add("❌ Материнская плата не подходит к корпусу по форм-фактору!");
                    }
                }

                // 4. Совместимость памяти (Материнская плата и ОЗУ)
                if (motherboard != null && ram.Any())
                {
                    var mbMemoryType = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.memorytypeid;
                    if (mbMemoryType.HasValue)
                    {
                        foreach (var r in ram)
                        {
                            var ramMemoryType = context.ram_.FirstOrDefault(rm => rm.id == r.id)?.memorytypeid;
                            if (ramMemoryType.HasValue && ramMemoryType != mbMemoryType)
                                errors.Add($"❌ Оперативная память '{r.name}' не совместима с материнской платой!");
                        }
                    }
                }

                // 5. Мощность БП и Видеокарты
                if (psu != null && gpu.Any())
                {
                    var psuPower = context.powersupply_.FirstOrDefault(p => p.id == psu.id)?.power;
                    if (psuPower.HasValue)
                    {
                        int totalGpuPower = 0;
                        foreach (var g in gpu)
                        {
                            var gpuPower = context.gpu_.FirstOrDefault(gp => gp.id == g.id)?.recommendpower;
                            if (gpuPower.HasValue)
                                totalGpuPower += gpuPower.Value;
                        }

                        if (psuPower < totalGpuPower)
                            errors.Add($"❌ Блок питания ({psuPower}W) слабее рекомендованного для видеокарт ({totalGpuPower}W)!");
                    }
                }
            }

            return errors;
        }
    }
}