using System;
using System.Collections.Generic;
using System.Linq;
using PR15;

namespace PR15.Services
{
    public static class CompatibilityChecker
    {
        public static List<string> CheckCompatibility(List<basepart_> selectedParts)
        {
            var errors = new List<string>();
            using (var context = Core.Context)
            {
                var cpu = selectedParts.FirstOrDefault(p => p.parttypeid == 1);
                var motherboard = selectedParts.FirstOrDefault(p => p.parttypeid == 4);
                var ram = selectedParts.Where(p => p.parttypeid == 3).ToList();
                var gpu = selectedParts.Where(p => p.parttypeid == 2).ToList();
                var psu = selectedParts.FirstOrDefault(p => p.parttypeid == 6);
                var cooler = selectedParts.FirstOrDefault(p => p.parttypeid == 7);
                var casePart = selectedParts.FirstOrDefault(p => p.parttypeid == 5);

                if (cpu != null && motherboard != null)
                {
                    var cpuSocket = context.cpu_.FirstOrDefault(c => c.id == cpu.id)?.socketid;
                    var mbSocket = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.socketid;

                    if (cpuSocket.HasValue && mbSocket.HasValue && cpuSocket != mbSocket)
                        errors.Add("Процессор и материнская плата имеют разные сокеты");
                }

                if (cpu != null && cooler != null)
                {
                    var cpuSocket = context.cpu_.FirstOrDefault(c => c.id == cpu.id)?.socketid;
                    if (cpuSocket.HasValue)
                    {
                        var isCompatible = context.socketprocessorcooler_.Any(spc =>
                            spc.socketid == cpuSocket && spc.processorcoolerid == cooler.id);

                        if (!isCompatible)
                            errors.Add("Кулер не совместим с сокетом процессора");
                    }
                }

                if (motherboard != null && casePart != null)
                {
                    var mbFormFactor = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.formfactorid;
                    if (mbFormFactor.HasValue)
                    {
                        var isCompatible = context.boardformfactorcase_.Any(bfc =>
                            bfc.caseid == casePart.id && bfc.formfactorid == mbFormFactor);

                        if (!isCompatible)
                            errors.Add("Материнская плата не подходит к корпусу по форм-фактору!");
                    }
                }

                if (motherboard != null && ram.Any())
                {
                    var mbMemoryType = context.motherboard_.FirstOrDefault(m => m.id == motherboard.id)?.memorytypeid;
                    if (mbMemoryType.HasValue)
                    {
                        foreach (var r in ram)
                        {
                            var ramMemoryType = context.ram_.FirstOrDefault(rm => rm.id == r.id)?.memorytypeid;
                            if (ramMemoryType.HasValue && ramMemoryType != mbMemoryType)
                                errors.Add($"Оперативная память '{r.name}' не совместима с материнской платой");
                        }
                    }
                }

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
                            errors.Add($"Блок питания ({psuPower}W) слабее рекомендованного для видеокарт ({totalGpuPower}W)(он взорвется)");
                    }
                }
            }

            return errors;
        }
    }
}