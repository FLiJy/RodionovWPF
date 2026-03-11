using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR15
{
    public partial class basepart_
    {
        public string DisplayName => $"{name} ({manufacturer_?.name ?? "Неизвестно"})";

        public string PriceFormatted => $"{price:N0} ₽";

        public string TypeName => parttype_?.name ?? "Другое";

        public string Description { get; set; }


        //Получать описание без basepart_ и entities
        public static string GetDescription(basepart_ part, PR15Entities context)
        {
            switch (part.parttypeid)
            {
                case 1: 
                    var cpu = context.cpu_.FirstOrDefault(c => c.id == part.id);
                    if (cpu != null)
                    {
                        var socket = context.socket_.FirstOrDefault(s => s.id == cpu.socketid);
                        return $"{socket?.name}, {cpu.numberofcores} ядер, {cpu.basecorefrequency}-{cpu.maxcorefrequency} ГГц, TDP {cpu.thermalpower}W";
                    }
                    break;

                case 2: 
                    var gpu = context.gpu_.FirstOrDefault(g => g.id == part.id);
                    if (gpu != null)
                    {
                        var gpuInterface = context.gpuinterface_.FirstOrDefault(gi => gi.id == gpu.gpuinterfaceid);
                        return $"{gpuInterface?.name}, {gpu.videomemory}GB, {gpu.chipfrequency}MHz, {gpu.memorybus}bit";
                    }
                    break;

                case 3: 
                    var ram = context.ram_.FirstOrDefault(r => r.id == part.id);
                    if (ram != null)
                    {
                        var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == ram.memorytypeid);
                        return $"{memoryType?.name}, {ram.capacity}GB ({ram.count}x{ram.capacity / ram.count}), {ram.ghz}MHz, {ram.timings}";
                    }
                    break;

                case 4: 
                    var mb = context.motherboard_.FirstOrDefault(m => m.id == part.id);
                    if (mb != null)
                    {
                        var socket = context.socket_.FirstOrDefault(s => s.id == mb.socketid);
                        var formFactor = context.formfactor_.FirstOrDefault(ff => ff.id == mb.formfactorid);
                        var memoryType = context.memorytype_.FirstOrDefault(mt => mt.id == mb.memorytypeid);
                        return $"{socket?.name}, {formFactor?.name}, {memoryType?.name}, {mb.memoryslots} слотов";
                    }
                    break;

                case 5: 
                    var caseItem = context.case_.FirstOrDefault(c => c.id == part.id);
                    if (caseItem != null)
                    {
                        var caseSize = context.casesize_.FirstOrDefault(cs => cs.id == caseItem.sizeid);
                        return $"{caseSize?.name}, {caseItem.expansionslots} слотов, {caseItem.fans} вентилятора";
                    }
                    break;

                case 6: 
                    var psu = context.powersupply_.FirstOrDefault(p => p.id == part.id);
                    if (psu != null)
                    {
                        var cert = context.certificate_.FirstOrDefault(c => c.id == psu.certificationid);
                        return $"{psu.power}W, {cert?.name}, вентилятор {psu.fandimensionid}mm";
                    }
                    break;

                case 7: 
                    var cooler = context.processorcooler_.FirstOrDefault(c => c.id == part.id);
                    if (cooler != null)
                    {
                        return $"{cooler.heatpipes} тепловых трубок, {cooler.minspeed}-{cooler.maxspeed} об/мин, {cooler.noiselevel}дБ";
                    }
                    break;

                case 8: 
                    var storage = context.storagedevice_.FirstOrDefault(s => s.id == part.id);
                    if (storage != null)
                    {
                        var storageType = context.storagedevicetype_.FirstOrDefault(st => st.id == storage.storagedevicetypeid);
                        var storageInterface = context.storagedeviceinterface_.FirstOrDefault(si => si.id == storage.storagedeviceinterfaceid);

                        if (storage.storagedevicetypeid == 1)
                        {
                            var ssd = context.ssd_.FirstOrDefault(s => s.id == part.id);
                            return $"{storageType?.name} {storageInterface?.name}, {storage.capacity}GB, TBW {ssd?.tbw}TB";
                        }
                        else if (storage.storagedevicetypeid == 2) 
                        {
                            var hdd = context.hdd_.FirstOrDefault(h => h.id == part.id);
                            return $"{storageType?.name} {storageInterface?.name}, {storage.capacity}GB, {hdd?.rotationspeed}RPM";
                        }
                    }
                    break;
            }

            return "Нет данных";
        }
    }
}