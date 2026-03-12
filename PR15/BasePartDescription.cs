using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR15
{
    public partial class basepart_
    {
        private string GetPartDescription()
        {
            using (var context = Core.Context)
            {
                switch (parttypeid)
                {
                    case 1: 
                        var cpu = context.cpu_.FirstOrDefault(c => c.id == id);
                        if (cpu != null)
                        {
                            var socket = context.socket_.FirstOrDefault(s => s.id == cpu.socketid);
                            return $"{socket?.name ?? "N/A"}, {cpu.numberofcores} ядер, {cpu.basecorefrequency}-{cpu.maxcorefrequency} ГГц, TDP {cpu.thermalpower}W";
                        }
                        break;

                    case 2: 
                        var gpu = context.gpu_.FirstOrDefault(g => g.id == id);
                        if (gpu != null)
                        {
                            var gpuInterface = context.gpuinterface_.FirstOrDefault(gi => gi.id == gpu.gpuinterfaceid);
                            return $"{gpuInterface?.name ?? "N/A"}, {gpu.videomemory}GB, {gpu.chipfrequency}MHz, {gpu.memorybus}bit";
                        }
                        break;

                    case 3: 
                        var ram = context.ram_.FirstOrDefault(r => r.id == id);
                        if (ram != null)
                        {
                            var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == ram.memorytypeid);
                            return $"{memoryType?.name ?? "N/A"}, {ram.capacity}GB ({ram.count}x{ram.capacity / ram.count}), {ram.ghz}MHz, {ram.timings}";
                        }
                        break;

                    case 4: 
                        var mb = context.motherboard_.FirstOrDefault(m => m.id == id);
                        if (mb != null)
                        {
                            var socket = context.socket_.FirstOrDefault(s => s.id == mb.socketid);
                            var formFactor = context.formfactor_.FirstOrDefault(ff => ff.id == mb.formfactorid);
                            var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == mb.memorytypeid);
                            return $"{socket?.name ?? "N/A"}, {formFactor?.name ?? "N/A"}, {memoryType?.name ?? "N/A"}, {mb.memoryslots} слотов";
                        }
                        break;

                    case 5: 
                        var caseItem = context.case_.FirstOrDefault(c => c.id == id);
                        if (caseItem != null)
                        {
                            var caseSize = context.casesize_.FirstOrDefault(cs => cs.id == caseItem.sizeid);
                            return $"{caseSize?.name ?? "N/A"}, {caseItem.expansionslots} слотов, {caseItem.fans} вентилятора";
                        }
                        break;

                    case 6: 
                        var psu = context.powersupply_.FirstOrDefault(p => p.id == id);
                        if (psu != null)
                        {
                            var cert = context.certificate_.FirstOrDefault(c => c.id == psu.certificationid);
                            var fanDim = context.fandimension_.FirstOrDefault(fd => fd.id == psu.fandimensionid);
                            return $"{psu.power}W, {cert?.name ?? "N/A"}, {fanDim?.name ?? "N/A"}";
                        }
                        break;

                    case 7: 
                        var cooler = context.processorcooler_.FirstOrDefault(c => c.id == id);
                        if (cooler != null)
                        {
                            var fanDim = context.fandimension_.FirstOrDefault(fd => fd.id == cooler.fandimensionid);
                            return $"{fanDim?.name ?? "N/A"}, {cooler.heatpipes} тепловых трубок, {cooler.minspeed}-{cooler.maxspeed} об/мин";
                        }
                        break;

                    case 8: 
                        var storage = context.storagedevice_.FirstOrDefault(s => s.id == id);
                        if (storage != null)
                        {
                            var storageType = context.storagedevicetype_.FirstOrDefault(st => st.id == storage.storagedevicetypeid);
                            var storageInterface = context.storagedeviceinterface_.FirstOrDefault(si => si.id == storage.storagedeviceinterfaceid);

                            if (storage.storagedevicetypeid == 1) 
                            {
                                var ssd = context.ssd_.FirstOrDefault(s => s.id == id);
                                return $"{storageType?.name ?? "N/A"} {storageInterface?.name ?? "N/A"}, {storage.capacity}GB, TBW {ssd?.tbw ?? 0}TB";
                            }
                            else if (storage.storagedevicetypeid == 2) 
                            {
                                var hdd = context.hdd_.FirstOrDefault(h => h.id == id);
                                return $"{storageType?.name ?? "N/A"} {storageInterface?.name ?? "N/A"}, {storage.capacity}GB, {hdd?.rotationspeed ?? 0}RPM";
                            }
                        }
                        break;
                }

                return "Нет данных";
            }
        }

        public string Description => GetPartDescription();
    }
}