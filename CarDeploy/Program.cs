//#define ini

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Car;
using Car.Dto;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;
using SubModel = Car.SubModel;

namespace CarDeploy
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var carContext = new CarContext()) {
                carContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (var carContextBrand in carContext.Brands) {
                    foreach (var series in carContextBrand.Series) {
                        foreach (var seriesModel in series.Models) {
                            foreach (var seriesModelSubModel in seriesModel.SubModels) {
                                //var propertyGroups = seriesModelSubModel.PropertyGroups;
                                //foreach (var propertyGroup in propertyGroups) {
                                //    foreach (var field in propertyGroup.PropertyFields) {
                                //        if (field.Value.Contains("&nbsp;")) {
                                //            field.Value = field.Value.Replace("&nbsp;", "");
                                //        }
                                //    }
                                //}
                                //seriesModelSubModel.Name = $"{seriesModelSubModel.Year}款 {seriesModelSubModel.Name}";
                                var list = new List<string>() {
                                    "基本参数",
                                    "车身",
                                    "发动机",
                                    "变速箱",
                                    "底盘转向",
                                    "车轮制动",
                                    "主/被动安全装备",
                                    "辅助/操控配置",
                                    "外部/防盗配置",
                                    "内部配置",
                                    "座椅配置",
                                    "多媒体配置",
                                    "灯光配置",
                                    "玻璃/后视镜",
                                    "空调/冰箱"
                                };
                                var groups = seriesModelSubModel.PropertyGroups;
                                var propertyGroups = list.Select(s => groups.FirstOrDefault(group => @group.Name == s)).ToList();
                                seriesModelSubModel.PropertyGroups = propertyGroups;
                            }
                        }
                    }
                }
                carContext.Configuration.AutoDetectChangesEnabled = true;
                carContext.SaveChanges();
            }
#if ini
            MapperInitialize();
            int count = 0;
            using (var carContext = new CarContext()) {
                carContext.Configuration.AutoDetectChangesEnabled = false;
                // carContext.Brands.RemoveRange(carContext.Brands);
                var list = carContext.Brands.Include(brand => brand.Series)
                    .ToList();
                var brands = new ConcurrentBag<Brand>(list);
                Thread.Sleep(10000);
                Parallel.For(0, 1, i => {
                    while (brands.TryTake(out Brand brand))
                    {
                        foreach (var brandSeries in brand.Series)
                        {
                            foreach (var brandSeriesModel in brandSeries.Models)
                            {
                                foreach (var subModel in brandSeriesModel.SubModels)
                                {
                                    using (var webClient = new WebClient())
                                    {
                                        var downloadString = webClient.DownloadString(
                                            $"https://carif.api.autohome.com.cn/Car/Spec_ParamListBySpecList.ashx?_callback=paramCallback&speclist={subModel.Code}");
                                        var propertyGroups = GetPropertyGroups(downloadString, CallbackType.Param);
                                        if (propertyGroups == null)
                                        {
                                            continue;
                                        }
                                        var s = webClient.DownloadString(
                                            $"https://carif.api.autohome.com.cn/Car/Config_ListBySpecIdList.ashx?_callback=configCallback&speclist={subModel.Code}");
                                        var groups = GetPropertyGroups(s, CallbackType.Config);
                                        if (groups == null)
                                        {
                                            continue;
                                        }
                                        var concat = groups.Concat(propertyGroups);
                                        subModel.PropertyFields = concat.ToList();
                                        Interlocked.Increment(ref count);
                                        Console.WriteLine($"fetched {count}");
                                        // Debugger.Break();
                                        //brandSeriesModel.SubModels = GetSubModels(downloadString);
                                    }
                                }
                            }
                        }
                    }
                });
                //读取已有表
                //using (var streamReader = new StreamReader(@"C:\Users\98197\Desktop\Untitled-1.json", Encoding.Default))
                //{
                //    var brands = JsonConvert.DeserializeObject<List<Brand>>(streamReader.ReadToEnd(), new JsonSerializerSettings()
                //    {
                //        StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                //    });
                //    carContext.Brands.AddRange(brands);
                //}
                carContext.Configuration.AutoDetectChangesEnabled = true;
                carContext.SaveChanges();
            }

#endif
            
            
            Console.WriteLine("operation succeed!");
            Console.ReadLine();
        }

        static void MapperInitialize()
        {
            Mapper.Initialize(expression => {
                expression.CreateMap<Car.Dto.SubModel, SubModel>();
                expression.CreateMap<ModelParam, PropertyGroup>()
                    .ForMember(group => @group.PropertyFields,
                        configurationExpression => configurationExpression.MapFrom(param =>
                            param.ParamItems.Select(Mapper.Map<ParamItem, PropertyField>).ToList()));
                expression.CreateMap<ParamItem, PropertyField>()
                    .ForMember(field => field.Value,
                        configurationExpression => configurationExpression.MapFrom(item => item.ValueItems[0].Value));
                expression.CreateMap<ConfigParam, PropertyGroup>()
                    .ForMember(group => @group.PropertyFields,
                        configurationExpression => configurationExpression.MapFrom(param => param.ConfigItems
                            .Select(Mapper.Map<ConfigItem, PropertyField>).ToList()));
                expression.CreateMap<ConfigItem, PropertyField>()
                    .ForMember(field => field.Value,
                        configurationExpression => configurationExpression.MapFrom(item => item.ConfigValueItems[0]
                            .Value));
            });
        }

        [Fact]
        public void ParseTest()
        {
            MapperInitialize();
            using (var webClient = new WebClient()) {
                var downloadString =
                    webClient.DownloadString(
                        $"https://carif.api.autohome.com.cn/Car/Spec_ParamListBySpecList.ashx?_callback=paramCallback&speclist=23562");
                var propertyGroups = GetPropertyGroups(downloadString, CallbackType.Param);
                //var subModels = GetSubModels(downloadString);
                Debugger.Break();
                Debug.WriteLine(propertyGroups.Count);
            }
        }

        //https://carif.api.autohome.com.cn/Car/Config_ListBySpecIdList.ashx?_callback=configCallback&speclist=32041
        //https://carif.api.autohome.com.cn/Car/Spec_ParamListBySpecList.ashx?_callback=paramCallback&speclist=23562
        static IList<PropertyGroup> GetPropertyGroups(string json, CallbackType callbackType)
        {
            switch (callbackType) {
                case CallbackType.Config:
                    var s = json.Substring(15, json.Length - 16);
                    var configCallback = JsonConvert.DeserializeObject<ConfigCallback>(s);
                    if (configCallback.Message != "成功") {
                        return null;
                    }
                    return configCallback.ConfigCallbackResult.ConfigParams
                        .Select(Mapper.Map<ConfigParam, PropertyGroup>)
                        .ToList();
                case CallbackType.Param:
                    var substring = json.Substring(14, json.Length - 15);
                    var paramCallback = JsonConvert.DeserializeObject<ParamCallback>(substring);
                    if (paramCallback.Message != "成功") {
                        return null;
                    }
                    return paramCallback.ParamCallbackResult.ModelParams
                        .Select(Mapper.Map<ModelParam, PropertyGroup>)
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }

        //https://car.autohome.com.cn/duibi/ashx/specComparehandler.ashx?type=2&seriesid=3895&format=json
        static IList<SubModel> GetSubModels(string json)
        {
            var subModels = new List<SubModel>();
            var model = JsonConvert.DeserializeObject<Car.Dto.ModelDto>(json,
                new JsonSerializerSettings() {StringEscapeHandling = StringEscapeHandling.EscapeNonAscii});
            foreach (var modelSubModelGroup0Dto in model.SubModelGroup0Dtos) {
                foreach (var subModelGroup1Dto in modelSubModelGroup0Dto.SubModelGroup1Dtos) {
                    foreach (var subModel1 in subModelGroup1Dto.SubModels) {
                        var newModel = Mapper.Map<Car.Dto.SubModel, SubModel>(subModel1);
                        newModel.IsAvailable = modelSubModelGroup0Dto.Name == "在售车型";
                        newModel.Year = subModelGroup1Dto.Code;
                        subModels.Add(newModel);
                    }
                }
            }
            return subModels;
        }
    }
}