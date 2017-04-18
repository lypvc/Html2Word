﻿using Novacode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Html2Word.Models
{
    public class WordDocument
    {
        public DocX Document { get; set; }
        public string FilePath { get; set; }

        public WordDocument()
        {
            FilePath = HttpContext.Current.Server.MapPath("~/Temp/temp.docx");
        }

        public void CreateDocument(string html)
        {
            using (DocX document = DocX.Create(FilePath))
            {
                //var base64 = GetBase64FromDataUri("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABG0AAADeCAIAAAF/GlrOAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAFVKSURBVHhe7Z3LlduwskU7Kg8UjQdvKRVPFcKde64wnEAncTO471QVUABJ8A+SoPrsZbNBEN/6oARJpL7+R26CqOq/tbGme4Rr5xL6zvi///u/cK0eoemMI3qZV9XX11dILcaG28MueWsTzf75NdVjseJYa6HvDBeiVZkYxtfXb0v8+/PLEmOEpjN6qvr69UeOscFtJFXZoP/+xt8wB6Tz/D///vvrS46WM4ENt4dd8rq/0NB//+IUgvA2Ld3r1xI4To3n729vBEdLgNB3BoSIq3+zkVgC0oSJWD7SGIkdrQwSGC6u4DgkNJ1hveAS2pJjUFWYLOaEdopNTSCq+s9//mMdHMo5vcwCIVoCk7fETortHNFLUJW2KOlJ3uHvJhb28vh6hlRgV6dDbGnCMPT43Wv9+fUIqcVoO306vbyfr2/kWVfvDV0AtJNUJY1GpOXanNPLLCZEI2Tto9jOEb1wAdxLsZ0jeimo6vl8wGHhpzgiEorPqu/i+MD511MzV7Owl8frpdePoivEt81HV6ftS1NIZYz3IuA0pBZTVtUYX5tm4tCrDI1bq1mnqp1QVXsYVdXj62HLEXxWreBt69O2JcLo9YKFTxfAr2hlb3RqXbjdfb92+XGRnhDD7OK/eLqCBaqyxg1ZZqNsVzCqqiOgV+2BqqrAlaqy12N4sRfOK1FSlayr+upIXlo+Hi/sf3Xxq9+701+avl82AORg9cM/WaDCGqWvCb9nXpEuUJW08/WU14E2ZbuEzNcjtB96/H5JjyXKqjoIetUeDlSVWlACp1TVHsqqCn4q7/t8wx+Rgp9mS1PvPbql5L283i8sNVjx5ORb0vgnS87carOfnhDRr04WPctMLY35anoRC1SlkrPXz3GysuKtXGa5AO5lgaoq8KNVVYvLVAVnxAsTJLARRVrXB0GcdB95L1hjdRGw/S9eGoWlQC8eyyWqsmlK4vGlEkVatvxJvnMUVHUc7XgVpl2X0HTGGV51HD2vCqkP5QjfvUZVsrQ+pGtZCJ6yDGKHKKdYEzVBhoi8xIerYk33gKrC5RMJfX8E5y1E5/juB0NV3YbzVBWWpHMJfX8E56mK7OS8lxXh2rmEvhXbIYQL9bDGneN6Ce3++vMv/5pxkYlLPWTIA/yb6PYF4K/ff/VvYOx76jvHE/pWMiF2us4pfq18uiNr3Ml6EX7HuktGO4G0jP/y/WxITlUF0Do0p03jKPnWTTh2RVxEhjzAvmWOY1CVtmYzQZt2VfN//fffHxz9++Kz47Eh+RfNpU4k9J2h2X/DV8kB6kqL0qjkxC+Xx6P0GE9R5S8KzHYBkI+xYEKobPMCeTtrv7KONkVVdbF2W6A4knOGd0QvwVtdZ/u/Mj42yp29dN8vXjSq4kiQCWDg4bzI7m+ZL+plJUFVwM73M9bU8l6Wvts8R7E7GwYI58cQ+qjaS021G3XHt4fiSM4Z3hG99FWln80Lj9c3jvJN6+/X6/XEImCfY80uPUtGOdZLuFyJJapC75iTvHdc9bsCY71gXZVvJmzqq+BV2u52FhrUzl6WsERVBzHZy+rwb9SX1zmyWEKrqtpIX1XPN15riZ/KZ0gw/PhayD5GWvIB85JRynczpb03Fr+nvBqUD6rilxoV+V7jRutzZlUVPxoLs9NOey81N9LvGmLUz+ew9MlndZteONGrDuGIXqiqQzhDVer+WA3imrCeJaMMq6veJIMevVP78H5P7zkLVYXede2VdQmjQv/Wt+TrIPVsHXkvWNnjV/BlNy0TjMugLfghU0E6pAb0VbWfIwxqGwtVdQRH9HKIqtRY6re8lg9XlSxD+hcuCWdUf5Q8X5rk4iRLRqnvAdp7a9gYivtbTrhciVlVoV+brKU1IftTTe+i1zWWc1nlsAWO39J8vV82W+T7GKbhAngIR/Ty41R1DmeoSvw0uP83kpbSVyxLlbpklNqyvOjC0Xbc2uf8IrCKRlSVZqe3Kuk05d7FKOel/MQFsC6h3S5j+Xu4WFW2scgTFTlCXhdyjYDCMiuLH14UyRiQwOvNbZ8OjPGBqsKU6mJN9wjXziX0/RHU96p2oKpuA1W1EV2Qzib0/RF8slcRcgnBqcJy0TA2ziWECj+AMGElZCn/p08OV+V+HXGXenXCHJSQFUHO7eaSnEq2oL/+/Pr1C3//xhsNkLajfMP+1x8cQPx6v3zh3zOB5cudCkp+F4Al9mDjXAIKo2sfBkBO+Cr/X73NQ47yLH4DGX4EqKt3W3i+TBOZocDvUBGnsYAc98sHZ7//yi0JHaARvbHC0D4TYcKTmCGi7j+bso7HqltCMvUB/fpXxxQHKZnZ+APIiZn5HENTdkmlhB4lU3Omb0kJY53D52JY7zZiG6q1ppfkaJPyYsiMOQeCcSan6mFjDScZR49pDBvnEkKFPqLmkIxA0Zun04J8woQnKa7uOvbCOOHAIXUuYaxzzEYqmdWCO8oOBeMMTtV7oIRltsOqITU+l1osnFrREMO1Nlg+nrvMpeBU8HbLNB5f8lHld/iOiDzJzD8kjl/dkEc52ldkJF8+w7jyo9yac9lUBQWsliNfaKwtFswupCbJDdGjk16R8aUBfb/s2znZsMNkUca/kPn9kjJ6hoP9kzSqoqSmVxPHM8/4XA4Z2AZsPMmpLNEmLrslND6XWiyUiRliOFFWCfMElo/nLnOhU92VhTKhU52JjWfGqRBEEVYRWPECQF4Y6K0L+GcvEvQgofb5JXdRIFNKItqmb7FIFLZIvIdVsqs4F5we8dMxVVgok1FDfOP1Xv7NS7x4kjsATQL61cyTWK7c9XMRJT4e+t36Nd9h3YyNh5HqriyUCSPVmdh4ZpzKN/oSf7r4JU8cxyrZzc6lh0akSPebopNT2x+Ad7FQJhOG+NV/aJKR3TJvaEgP6QNYrtzxuQzG3OVxVuC18YShTBsijvJK6S2J/OekYI3IkQgrmWJkdop/UvH7VeU+XrBc7mB6LjY2OT7kuTb28g85dtVeJ9gULF/e1tOXFnqTsDwSwqZ09M+rzrJQJlOGqDOSSSnyMApJ2205QSYAOZCSJg5huXInnSqNWQYc7E4miNFj/LndHoeNJ/QEQ8R5y9g4l9D+XGoRJjyJGWLjhLHOcZe5JKeyRA94PLw8nFyHjXUhY3MBCDgZspp1cwzJzze1WqZOyK3IQplMrO56TIwp2vJ7b110N/27hLNcuRNzGXs/aWxS9tIjnNTDxjPjVI2wXO6g8bnUYqFMxg2xFZaP54S5lBbZFdh4buNU8eWy/SugV+VIp8qhU52JjWfeqXovmXA29pIJO107AXVfMq2S3UKnwgsDnUcaJMaMf4/wnlj91wZ1WSiTCUMs6FE/3snyIYSOfI5guXLH59JX1vRL2d7V7ukuvdt45p2qBZbLHTBS5TBSnYmNh051VxbKBIYYUq2yXLkf4lSIjPj3fobtyv/+921vmSDtmfYuCg6SRpnv8ATCiqyS3YRT2ZglxOsHmvp6Rj99iqcv+TxGfzpBplb8bLQVFsrkhzgVdPeU5wFDcaJQffSTmKGoM2ZajmTqB3RHfHJl4wntMlLdjoUyYaQ6ExsPnequtGZPJ3CXBeIHOZW+lJX5hnex9J0uvAxAfspU9PWsfDcH/8KX2NvjZzpV+2CcjFR35Qc61V0ITvVJ0KnItdCp7gqdqlnoVHeFTtUsH+hUhFwLnYqQmtCjCKmJeBRelzeOvfG/hFDhBxAmPIJ+H0cIpRvGxjnBveZCj7orZmd2hHxCrqJqDf9CVsNMTATcay4yEfwPZ/qgfUv4jybIb2bMPGX/X+kh8MXM7az1qK8v/UUP/aUJkE9hbjpet0MxU6gnH7Qz2kuJMOER0JYlQuls4v67A9rhlKL+/CpMrZiJljZr3MY5wXAuOfZDCtCDnQKf6fnIaONAyz9DYgLHEfn2Wy/IkD/ZT8jY75GEnPCjKTEz+xWWPaz2KP29lkN/LcYarCgfHFE4S8/84kuY8BwoaV0YOEVrQs2fEZKEXwXeoxTWLFQZIwx0AVpcerFfeBIGv/aEzqCOfMrC5E8QVQSDTDEK3Zr+4PNuMdK9it4GBNnBYlTBMmSIG5fs1BM2YkmrxDE3nKrhbWebR9lofUiSrwkbmM1LjFYL+zhj3eiWOscsLQaJWjhqoqZ8cAqslmghaOSvdPnff3pMhAnPYYW1tWB23o6PGWkk0FfqWsdm5XGaD0ky/4l5pDmqBHRZkRyvZW1KuhTWnDDQBWhxGb4Nz0alnSSPwiUbjwxgoEFJ6PEgMMjOq74cdCyDy/j7+7cN7nzWetQQmU5JlPaaYQPS3KXyCROeI5TuoAtKF8ykKJ8TCANdQKgwgjjWRVNwMMjgUTrgRsHwVnlUSH00y78d0rhAVn3N5RZzSR6FoyPXmwHjWetROomA5X8Yaz3KRAEssx02eJRNBFhmO5Q9yuKmXP+f3D1uiQl69xM99SbzujcZYTzbPGrnXDZU+Xqkpz3bjVhGdbFs8ChIw9RtyF1h3UfDz6LDD/ec53MJWZvY5lG9uRwxsA2UPQo5dgqiSckDG3DMfvzvLTfw9X9aD+OXp1LjLJ/VfjCebR61cy4bquBPfKxp+JVB/Pt+4VhZLBs8ShPJCs2j5CZL/SlBHbRMAWlMIU1Wvc4mouf6KPk4cfyDcduEt7HNo3pzUSoPbANlj8rX9Zz4OJQLwHj2x6icDXO5cPpFtnkUsMweeWg9mc0eBSyzHfoe1SwYHt+Z6LHWo5plg0c1Cz3qxtCjGoQedWPoUQ0y71HYnurfN3Z4j9fL9hL6Nos9ajA8ZNB2gZ5pb73Ilvf1rc8flC1j3NduoYpHbZjL80uetoicBtnvUdBOeAZonLjv5lUCJ1HFo8bmAj3K20vv5+stG8Wj5zXvURgK/tu/bwwNlhjMCwb5xEGydPSWqf+E5+OBGcKI5bIUBhd71Ia5IP9pv5/dHvs9SpE3LW3iNlNIJqjwLKp4lFKYi+pR0sg9YV4LPCrxveZpxmKDe4JSjzoelVg1l0ap5FHXU8+jythHIHCqE1jlUVdS26M+AXpUg9Cjbgw9qkHmPUr2QiPxEpfCdwPsRd6R1PKo8sa0+02cznT9R8q7yL7rrBcSY+z3qLFt+jD/0JlW8ajiXLCnCqmzWOJRGJN8mwZ+pVKFgdkbd+GnYlTWmolN4es7nL6f8fs7dajiUZiD/pXR6p7VDSV4DaaJnJQp72SEE5ziqv3DSQvvWOz3KNWRzN0nbjOSxMPe4UxXVRqHUMWj0pjjONVEO+8/md0euoVe4lE6yu+XvSlpmYhbZluwy/DmuC4SKBlP5R9srrhybKCKRwEdj4wWco5GA2Rq31g4ZDaSGYvJW5S4LHN/PHGCDFGSqEpUE6tfw36P0mmCMHGbkS6Vkq+qDFdlpiPhej9VPCqNWceJUzPR51NWQHs15XYrFY5h3qMaoZZHfRL7PaoRqnhUI3Q8qnFWedRPYJVHtcxaj2qZjkfpgBsFw2OM6rHKo0KqSdZ6VEg1ybxHHfqiczlVPErn0tkLYPvQyzEeD5l1tk2SMvk2ogWx7PcoTLA3EcyxODUVRZr/sEwmq9VU8SgMYHoIgzFjOgXVG5v1u8ijfFeHQds/2wWeSUWPks3r98t24fgnYn37fYT6Hoxeej/jGw96ryHqWnlcxUZ9s8QrUsujMDF9syWYl2hc9GsGJ+KSS98vua4fM9j0XTi4jmQ43UQVj9Ixh8RAxZKJMeMfJhEnqn+0mErA3wCU/EHhpcx7VCNU8agPY79HNUIVj6rHSh/qQo+6MfSoBrmZRyFAA43hIcQ7lmNHelQPetRp3M+j9KzvToY7FT2qBz3qNOY9Cvuz3itLvXOr8FrTPpzOdqhSJt+walMbMY8KJ3OMzQW7T/s+XhqJfmOlO+Y0tXzwDVLDowp61El3hIB/frOmHitTxaMwyJ6Bjb2BJG+qZN+oGpbZo/d5j3o9HvYOjx1Nvs93/pytdAlzCJnd98fAzvfHqngUhqVjka8p+uC/XyphSeJg/yTXHlJ1iAVVopJHdZ7IFV8FdDLxL7zhq/laoCa1PAqD1K/FdbwlDFv/xUtqBjoRM0vU1WzJQTKcbmLeoxqhkkd9FDU8qgmqeFQj0KNuDD2qQehRN4Ye1SD0qBtDj2qQpR5l98xgD4dNnCZQ5Y3NqmXaEZs6uYMo5oealajiUS+MNI4Zu08dpGxcXy/ZsPqppLWYfJmlYehRXeS9B/yH7lyhULPf4faU58Tp95Lkr9mwGLZUrcdSjxL0gXvqOfq+ir0hEjP1n8xgz/skE1TxqICOOY5TPSq+IWla+V98QpUXahN6VJeO7vDPbtPuZeKffEvRy9dmjUddSk2P+hToUQ1Cj7oxqzyqccJAF7Cq8PncxqMAParHqqX9Y6BHVWO/R8mGdPhmw+CBJNgQNr6DMuhRDfJjY9Rb3keJ30nRI5BMe5fFMht3q5/pUTCDxsEgf5xHwWvMc8Sr4rurlgmvQo68KaTPHrO30a1Wa/xMj7oFPzBGfQL0qGYJHtU4qzzqJ0CPapZGX9VsBtYWUh8NPapZ6FG3hB7VLPSoW0KPahZ61C2hRzULPeqW0KOa5QM96ocQJkwa49M8ipBroUcRQghplxSl5Kt+ZDdBmvUI7ZIPIqi2BqFFso8gzXqEdsk+TJgpSoW3kcgOXKwVCU2TDwJqtVsbhsf/jIBL6q3yz0oaMLnQKNkBJDnUhR2DAsZBQftn5Q3qpQomzCTWkE12wChFlgC1DldDO8KEiuCSeqv8y8Gl0CjZgct/eDT5T4CC9i8H+aFpsgPIXxRhf0DI/u9///359fX1G4nfX1+//vyzTPBXz8OJIjn6002WSkVrg8b/hmTTqMlWJjSdEaXxNwg/MlQBckyDSPR0VxE3mLW0aT8qKhnLn18yqWLdzVM2gmprkK+Gcaj3NAy1AU39wyzG6qLMclUuJ0izHrNRavky68o6ziCbxYS5KErJ+d+y+JCfp70KTk3yQaa2IAm/0AP+IM87wqlpSQpZF53ysZFYEkdr3Aamzch1L3YVh0Ypl3+cZlyM/v2xBI4mxoDKsJuOVbQt/M1VYECwPXl6v1bdutCyAsp7I6FkbDwOrK9uB/mt2o/MdFjXyKecD9haBj6pMYJqazAVpW5oGDKabl0boZKrIww4a1mQK5sI0qxHppcwzVx6kNXQhOx6mFGfYw2yWUyYhSgFZGa//tiEfZZF8al0BDt1GZluUBEHVDYrsiZcbS5ZnELOUsL4/bdbPlQ3oVvrch6ln6v/WnWcE6WiNFS2JihFM/vI5aSU4NvA9JarwJSFgj15Wr9W3uiVD6e//vgIwxBDX3115zRoP1pWkGvdusMp47r+ld5DmTSFUYJqa5BHqThUFZqmLV8z+8jlNFSpookLDUNqCaaVvC6KaRPItFO0bQ1Zyz0r2kCQZj1mo5QlMHibgaT1qs1IrwdspsBOXQLyp5JBNosJsxOlwqeBZBMQ4EFRKnRAPgL3vSrYahiaJpuoqxGDetmP66UfpcIJWQmkB46LUuGE3Jzc96rgr9nDeYm6PX4Y1TViUC87mYlSItqIbRgB0lYsxy6Fk1m+x38WcfzSM/3Y47vl5/2buA6NUtaFYZIHSFuxHLsUTmbZpBdwRi/6OxDLf5+wfWs5NEpZwjDtIGH5WjaiUl3+qzQP/bnTIhOXtP/lv9QqPwNr6O97nMcJUcoShoqlrBe9cgOJncO6KGXSQ9qKOY/HyxKvB8q81ejlN5q+X27/slKE38QQ6YeFw5cSFa5lppJq9/Kr2dqYFe412yImrjOj1LV6OacXRDCdqRU2rLCcpuPb25SOusNoi9OiFKSOf5qQfC2rfLtgviEoE3tUUxCyqSbzQcv3ZbGnAi2kLzUydYthSH7UO7DCepqOVghp7agzjHM4OUqN6uU+EjuHFVEKiBDmX7MHyeL08RJ7hcS/Hk8kcmsWESvfuvo8H3IJiVD9+4W0tpNUYosa8q3ZZjFZnRalwLV6sQJappNTW/uCnCD1fHuzdqp/5Nf1LKFO27q1nBalVG7l1TDITdc6U1CSZFJNrgX5wTUBYu2qwKojD7nemh7lF9msD/zNmrVTtYSntCMJfW0hRdMwzuPkKIUJjujlNhI7h5koRTYA6YFDoxT5AA6NUmPU7fHDOCFKjXFEvx/DVJQSuZKtHBelrH1yd46LUmQbR0cpso2pKBVOyEpUsNxLkRmOi1LhvETdHj+Mo6NUOC9xRL8fQ6Uo9ZY3Sf/3v+99n1Pb299TLHjX1T/XXVL4EMQeG4lSp+jFvwSxRIPEuT5Kfb/EWb5fA8WN6fF65zqUVqLUar2AT3a9OlHqEdepl3/WGr4+pGn9WA8Js2w9Bpnax7b6GaBnytEaRL08E8QW5ANDZNipFNPP2wHO0KR96+wqRxJ7bCNKnaMXUUfeLFnG9VFKeajXmOKGGrejlpH05c51KK1EKWWNXkDIhIKeoqBQEY6s32cJ38W4KXWi1Cj2ouAATOZtuorYY+Pv+B2mF7KcRqIUcZqKUtvQV5Dy16LUZ3BwlPqRiD3ycykyB6NUa3xAlPpIGKXqI/bIKEXmYJRqDUapNmGUqo/YI6MUmYNRqjUYpdqkTpQKnw91P+3IPjQqv0lqBbqfLX3C26lij21EqZc8skjwr1FsYkYpX/EJSb2PCXunDr8TaFwfpd7PKP6eIsb0EvK75lQoPKb6xmkmShXlvIibSn6a+lHKJWuZulCq0GMMe8oTw+SSHeVL0im8dUqWT/Xovexbfw9B7LHFKBWEKSJMMpTzTB1axh6Fp2JPXw7MxD787iXInpEzbDYcv18P1T6/EyhcH6UUKCUooqRx19FX9pUzUXHJSb1uru4b0WKUSnIO8jQfBJlnWXl7GGPMjBV76+0dqROlSI7YY9vv+CGeqBeQK2kkShGnmSi1lhDVPhVGqfqIPfJzKTIHo1Rr3DZKfTiMUvURe2SUInMwSrUGo1SbMErVR+yRUYrMwSjVGoxSbVInSj31V2csgWN8IMf79ZYHF33LD9Fo4/oco1t9nroFscc2olT8vFTetlYliBb071M+nHo+3npJn/InCSjRyqAiikGHOLVnAHqt17c9zO0bCbmiOhVtaxn7MSdU15bQVCojRazwbT/FrUsDUUq1E+xEf8QISorKQiYycFzl2ihm3+F8PZCQKmo5qYxmihUhDdPKykgtN6FLaCZKmV7kCPmIZKJ4h178eomrIaEZ7+jC4bSnAm3NPDqo3r5gFVuLNhAT1rs2EjS+oOX6VItS8kdEKT9Dh5HaD626rO0IAUqxT0fssY0oBVOC7G2VAfZtrmhJuXbE2pCAlaGQakoUmh27td5PN3TTqX7vKP0wrlXRGqkMsIQYCWkiSoW7CGxlAlBepqxkA8td28r7yoWr0rZ+3yzTvlWRklbGrctN6BIai1JClFIQLxjz4iD5ggvnKkiZVlE1760JSFvoAnl5KTNopNRyfepEKZIj9sh3/MgcLUQpktNMlGqQFDjPh1GqPmKPgyilry7t31LsBaklLE29fBKMUq3BKNUmjFL1EXvUKKWhRf55wtKQ7QRoQcsLUjpLUy+fBFQJhYaTGnA13El1jRjUy05yvYTVECBr1WoY346c2BW+43vfC3eOXl7ehy1VkXb0LXJZvr/1rdgeB71JOovYYxvv+AXB2bMkFJFJJisTkYbCVKaEfmQa4yWpwuVRyj5pB0UH66EFP9xImolSYZGcWMF6HwTOkn+yVfxgOK0M8sWIly2tXS57069+lOpEI1kf7aM5OdViPlXLkY/3zbLVB1ItuaofzX3Hkihjn9wif+RTehkGLljhqz6lF3tsI0rpB63ht7gyESV5mohgklrGJClYMVecX+p+rJpdygIhWcjlUUoJSvRAZZoVg9CPxv3F4k8wkhajVBSaCjMtbi5by/cc+faEfblJ1ZfVCt/VfDxeVlIuh+9Z+FKQGrQyMgxdjVF4ZLdwBnWiFKYrL6Ls+0KCSDZ+SdEkHuRuU4Udfz3kx84ziYRjt9a3uoim9buY1ggS/o3nTvtaxhxM/mqD5yP22My3J+w1UVfgKqskovDVUhTDX1PlMz3MzTLtZbLJ0wvYJVcZWUdTUUqPPdXLUXUr65T+/XAjaSZKqcPqK3jzU5F7XNxsWfPFTQVeUFw4dmu9HrIyW1p70EaypQBV8vZda1K0jTsEkg0ha+1qSByxx2aiFGmWNqIUSbQTpUjOVJQSuZKtHBelrH1yd46LUmQbR0cpso2pKBVOyEpUsNxLkRkOilLhhKzn6CgVzksc0e/HUC1K2fuYs9iPo7ziT570yN8Stc/0nIXtt4DYYxtRyj5IeMdnT4CeGPU0fLQwJLuavnKpjFYhy2GUao2WotQyF7Mfkfp+jZQPznt3F64cpcIiqB/HQQopM/+CyPcL4hot6Y2gHGp0C9ineV1xN4fYYyN7KRHhUIxioA/9qoufKsmmh1f1B+7kQ1T9xFUz7YPZ5j8SbxZGqdZoL0oF71M/c2eUI1ZCuaBoerSkX7qvC1eLUsQRe+Q7fmQORqnWaClKkQSjVH3EHhmlyByMUq3BKNUmjFKHAAEySpFpjohSZD9BmvVAm3UV/dNglDoECPAIc6dePonqUYq0CaPUThilDgECbCRKxa+izGDPDQHF8pYZLtnXiiIL2ydDGKV+CBalyE5MmIxS1YAAm4pSdtTbAOQrf3nm9+vhX5n0J6kUS9rRolSvgN1gQFbBKEXIWhilqtFOlCLNwihFyFoYparBKEVmYZQiZC0pSmGFJfsJ0qwHo9QnwShFyFpSlCJtwij1STBKEbIWRqnWYZT6JBilCFkLo1TrMEp9EoxShKyFUap1GKU+CUYpQtbCKNU6jFKfBKMUIWthlGqd8N1B8kEE1RJCFsAoRQghpFEYogghhDQKQxQhhJBGCSEqvE1O9mHCrEhol3wWQbs1CC2SfQRp1iO0S/YBSaYQ9V+yGxNmRaiXz8McbwNfSjiJhEbJDjZrxKBeDoIhqj4mzIpQL58HdOormq9udvzPOFrA/yVCo2QH2zRiaDH/lwhNkx0wRNXHhFkR6uXzMMcbYmtiKNQlXsXBlsL0L5QgO9igEYd6OQ6GqPqYMCtCvXwe0KmtfUDWNk3bEZfG0AL+z8pLIjRKdhDFKyBhaTua8CfQYv6PeqkJxCsixX+Ak5Cd+Atxh6SC038hmUCm8PtvOF/Pn19SP5xMsrzkVZgwK5Lp5R8mn0v599fXrz9JIX/1PJw4kiuE0y30+x1necmEjq4t+/mn5cD2MU1ijrcBG1U4iYRGM+5rGDoicLaPb9aIYYMOJ5HQ9Dzzy+zRBtksoyEqymJedsgxZ0Ci4AaVUA01HZkcE2ZFcr0YLo3ZlchWIU/1dFcRNL7NedRqGrMflZWm/mFSY3U3TxnsXBB7hEbvbxgobMP+80tmUax70FJQVyNGaHqcOMG5ZfZ4g2yWcogSCSj/VHYBNXH8HZGdp0MVFLOEEgzOQNq7ACgpVWGbWW63vLwEE379sZL5VX31HQuAw5a5hZgwK5KHKMzvXyYNEcavIDQXYyiqIN83J5YWJQGTZARF0mlJnvjbUejvv3n5zGBCybxxVO+pO6HluukW7CeMAcX6dY3OlMP6ElqOhVzsRY4JUbc3DLtoA+vVNfpzjC9uumW2cGiIyqcZGJhQIJNqxuEG2SzlEAUwJZ1RmC0CtyVw7MhO599NxyoqIfz1mI98A3aledIDTvEnSFOQ6taFlhVQ3hsJJWPjcWCiNj0LxS7EhFmRQYhK01RhiLRMnJkYAyjvpmlpr4JT/SsJKSKVDQi2L0+cmlKkkHXRKR8biSVxtMZtYD11J7SRbroV+5HRdOvaCJV8ymHAWcuCXBnnmBCVhqpjETGYnMLYMjBClV9KexWc6t84wc6sTzAMkeewrpGrIx+wtQx8Ums5PETJyIL0XB041fFGmx9bZhUTay6BEVFvMchmmQpRqn2ZLaat0wxS6MlOLkRDUTVEcWsd/FUJ/RaxRq2geM80gzTjVdArr1qRAYSSUZcx/weGKJGGydbyISUXo6NK6KvD9aWJoGIc5URKFeTZvSr0cvDHFGf5qOb6Rb42k9Sdg5zm7OdvMCFkohOvi9PSlGXA+KvZQfhao6OIHkeGKBmbCQ0JG6TLx9FZ9OV8uWFY2odkZaSvkiq1B+QH+SsyvJhex1UhSoUTRJ0LrTOL4w2yWUZDlE4b81TZadryNbOPXBYJ2BWpogmVC8qrfnAqJ7/lKgpGnUlm0M2vP1be6JUPp9kbNWGIoa++51yICbMiwxBlCchBhSFzN2Em4WSoTAQ7tSqaDBVxiO7xZU0M5YlTLA5Swojv58TyoTrataNVAbbm9NTdw0q2ZD9SS7DR53VLU7aGrOWetMc4KEQB6fu2hqFlBbnWrWvjyVWJ6/pXeg9l0hRWc0mIihNUUWtai8tcutM43CCbZTRETRLlNTdtEyeKbbabO2LCrMhivQTMQIVo0EX+/pYlxizYcs6C9nNgiJqgecO4kkND1CaWuslnUwhR4YZpsgkI0IRZEerlw3DHqwXNYyfVNWJQLztxvfR3UXZaZPrqD8dkGk7qQb18EqbKI0JUOCErOUIjBvWyh1wvDFF1YIgis5gqGaLa4QiNGNTLHnK9FEKUJXpYvpYlBU4IUZboYflalrSOqeygEGWND7FipIiJ6NAQZV0MsWKkiIloUYjyj+wsX8tG3k9cery+w+kcj69HSA0Yv/R+h8T/nuPVW+DkEHW1Xk7qRWf5DCfztG4tqrozQpTKTUDaiuXYpXAyy/drVMkTl9Q8XB2zPJOW34ttqgImrtNClEkeIG3FEjeR2DmYuOZDFESmBUSmlq9lje+nifP7tVysQ+aWEoaoNvVySi/vZ3S9nhOO+SRDVCDaRmEpfD3EZsBjRewfMrMs4sWLJXqKGNPLDwlRE3q5i8TOwcQ1H6KAFii9Wv92schqhRcASNkRr60134T7/sLltyxoUda+johkLdNKSiF9XRbbgQ6kcK/ZNjkzRIGL9XKS9mU9lfzMCa2wnqajFUJaO+oMoylUdWeEKFsHAdJWzHnEpVBj1Vv3wSL875dviU01roUgfF8TeyqwkroVTuqGLnUMVtiwwnKajm9vUzrqDuMMTFynhagJvdxFYudg4loUokymSFi+ljU6r6NNgmb9ktbM1wOZuSlLGVtN9BWcrHNJ9PH1OK6nTF10UrMNc3KIulov5/QiwANDO6Vm41H8HmkUaNlaVHVnhCigIikshd1dVBCsilDSBdVYGVsZSyrQKrgu8vdMA+q2dkrNhiP0q0r31z2p+gmYrE4LUWBML0b7EjsHk9XOEKVWiVdZurSY7EKOpOUVwXOwSOl1vMB9wU++Hk9kSk7Silzz1vSoL7pTs+3STIiK4jpWL+f08pY87QN/s2btVH39Ke1IQly6dWtR1V0cooBcUAFGBUkO1IK0qybXQpTw13dfBVr9+4W0tpOrW5ATVXFP40lHmoDWbIuGtA3jNExWLYSou0jsHExWi0KUY/ladgbILKR+EieHKMfytewM5+jlZ2p/Iaayc0IUFi9LWLFZsPR93EI3j4nozBC1Vi8/ExPRaIgaY/rqD+eEEDXGEf2SIzBVHhSiyAaO0IhBvewh1wtDVB0YosgspkqGqHY4QiMG9bKHXC/9EEW2cXSIIh/DESHKWibbOC5EWftkG+UQZadFpq/+cI4OUeG8xBH9kiMwVR4RosIJWckRGjGolz3kemGIqgNDFJnFVMkQ1Q5HaMSgXvaQ64Uhqg4MUWQWUyVDVDscoRGDetlDrpftIepL75x4xTsBt7Hg/pVw68YEdouAMl/4INoJUafoRe9AUhZokARMldeGKLvT9hVvkXbG9NiCcx3HERoxjtYL+GDXy/WyNUTFxwFISm/0+4KI9SY+ExwSetPl+/WSe82Qg/xvuSFN8vX+M2SGezOtil4LJZGIj/UQxwgLrjy07f16Sy9yqt35XWxyr9t1d7G1EqJO0ouGqKxZsgRT5bUhChrWv0HFRY3rUe/StacYXO1cx3GERoyj9eKnloZ6cKpqEiWZP8uFe5LrxeRSYSlEKn/2jIhIEvbKS46WD2HjaPdIe6YcY4PqFTFTCC3g6lPaDy/lUMyWTtXf9c8CaTZEIXWAXkQdLT9qqE1MldeGKOH75dosaDzq3Ra7FpzrOI7QiHG0XrJMhKI3suxUHFlfOwItdUtyvWwNUbLYSZSGIPVowpK0rID6jGp7sqTK05fC8AYRYj3k2FVAeJAoDlkmCI7x0MeB4FQ0FIqFwZuSUD1T39m0EqJO0ou+YvBmyTJMldeGKNX7N/zEtNnTuL1d4TrVYtc713EcoRHjaL3gaJdQzB5JZf6obxja04FvTK4XmRXYsBSOYW+qVie8zI8RqzXaCVFjHKQXshxT5bUhiuQcoRHjTL3Ii0iPWB9BrpeaIco+0bDX43VByzEyMUT1me33OL2QVZgqGaLa4QiNGGfp5du3VgxRUi2kyICWQxRpBFMlQ1Q7HKERg3rZQ64Xhqg6MESRWUyVDFHtcIRGDOplD7leGKLqwBBFZjFVMkS1wxEaMaiXPeR6YYiqA0MUmcVUyRDVDkdoxKBe9pDrhSGqDgxRZBZTJUNUOxyhEYN62UOul80hSm97jncmRWa/bleq9X7KTQA3p6EQpTf9Lb8xQm+TKjNxSdW4/Fvs4b4r8EnfO1qLqfLiEKXmsfzrnVJ6zpyyO+1uxhEaMdbqZYmcA3q/VJ9i5m3J9bI9RJlEunfbJDMdWYmsQMeaP2PNaiZEhQeiwGT3fMt8Ril6C7Cluo7RO3UYogRT5bUhyl8O9hQxppeQ3zWnUuEx1TfNERox1uqlKOdl3FLy0+R62Rui9LZnTb/lrmZxgM7jCaR9e5GlLxDCI6f0khRAZizpp1Kld6ov2OXWa6Sz7hqilRAVnlUjKYg5CtNkKCLVUyTeogV9cISJGjnRN0S8mVLC0yU003Uhelc1JvewwnqajuFpYnwahWKqvDZEQZdwQfyJegc9jesx+lcsZuYkafdZxevm6r4NR2jEWKuXopyjPNPSpwmT+RdSgyfFiGYt0x38juR6kSmBzSFKdlEp8ltmJmVdzvTBEEmO3690CeQlx07l6L3s2x8cRCshqruLCsLUF84uw5c8LiWpw8qYM6jY08P9crG7Tu0IVI/aTqnZeJS3BJFGAT7Tz1R5dYgSoG5TREHj8Qj9utJNxSUn9bq5um/DERox1uqlKGcg8ow+mHuWlU87BD1axd56e0dyvQR72hCiIDt/RWxPdBXhPCUplxHQYeD6QMNMWEFkttmSOtFVpk/jUUAidtcQzYQoE73IHgRhWo6k9THJgxCl1+WNcHkOhT4LUXLSKibXvDU9pt0w/mbNhhVKr0o7khC30S1aGsYPxVR5bYgyVSNhKu5pPOlIE9CalVdrSV7Zq2smkan7NhyhEWObXnpy1jyRp101IdsRMpdr6ubpklb0MvF4P3K9iLGCLUshyWgoRI3gH0KQqzBVXhuiSM4RGjGO1stnv9TL9cIQVYf2QxS5HFMlQ1Q7HKERg3rZQ64Xhqg6MESRWUyVDFHtcIRGDOplD7le+iGKbOPoEEU+hiNClLVMtnFciLL2yTbKIcpOi0xf/eEcHaLCeYkj+iVHYKo8IkSFE7KSIzRiUC97yPXCEFUHhigyi6mSIaodjtCIQb3sIdcLQ1QdGKLILKZKhqh2OEIjBvWyh1wvm0PU+6E/p3/fr97XpZkQFdQx8Z1UXOrcNTUHikUVv+0GqR7ezlNvuClBIxFMldeGqKisCY2sdW2UTwZQquLtjD7tYKEpVucIjRjb9LLyzpCegmb0ZV2MO+kECy2hGrletocoDNqfxqH3+UlTkMJDbyjDqS1seuPnne7m20aTIUrvqMWioDf06W2Yohwcoa9Yxm6zxcX36y3FvuWGTLWKrNZTNfh6ICFVVKepjGaiX7nTEErPykgtJB6vj3rG5WZMle2EKFW16DEqS269f+ulNa4tqtfQ842EXDGr0CzXPqprS2gqlZEiyYQu4AiNGNv0okfTgou36MXmaAJqyR+50d5Oeyp4v15yxbvAtbw1t4GYSDaA8qo+72i05erkegkdbFwK9bl8lsBYbVa4lo46n4Om0RQthihEGz1R2zMjTtrxU72GhFVMx04tffQf7NLSYp96mj3TSKpYg1bGaltFH9UPx1TZToiCiiWZnDRpf41ra1OPFwqGtL40kdNM+1pRGwxllCd2YFc+FusIjRjr9SKoZ6mUMg8a92KTZ8GFi09CikIWrST5Rxswpx6W7zVyzjOWcr3o+HYshZghEgi1mCQSmfjsaM89/HyaCVHhvQIYn53aiyBL59pRH5Dn3EA9vYdR2rFXC/Yc0+HBsjhYGXePrH3XuxSOXvbTMVVeG6KwuEAVusQIUE2uLP1rx+WureXfTz1NlqNrWdK+VdEaqQzITOgCjtCIsVYvJlglSCn3GaR7/pjkWXLhXAVZplWUS94aUBvoPqR7qpFCZnVyvYSBblgKSU47IYo0i6ny2hBFco7QiPEBesmi5tnkemGIqgNDFJnFVMkQ1Q5HaMT4AL0wRH0UDFFkFlMlQ1Q7HKERg3rZQ64Xhqg6jIQoiFc+lQxnc+QlLU29fBKmSoaodjhCIwb1sodcLwxRdfAQJRFJUdn6vxnGnIR6+SRMlXUXRDTIpXAzR2jEoF72kOslrJ5cCnfiIQoiHcQn/JMCE4w5CfXySZgq6y6IaBD2E07ISo7QiEG97CHXi6yegEvhTmCOJflIfNKItYi8pKWpl0/CVMkQ1Q5HaMSgXvaQ62VziApfh5/41od/9X7hN0Pyr+oX75OQdvQWDbmt4/HSWwF6HPIl/SWMhKi9rNWL3+/i90WZTDJZqYhQLtwnOIqUgBquEugnYqq8NkSpVoVwPsEPMJIjNGKs1UtcJCdWsHd06oWrnJcPt6YNSCsDlPhtS2uXhUt3dXK9HBqiHqtuHZfy1uhbHtKjeR08c7zBhcqrTyMhCsaoVmkPpDF6MpFTu70XTMg5XEq3nQvjkifzmCqvDVFgoRJ/gpEcoRFjrV6i0CZWMHlEma2Py1Y5eZSRpb6ez1IVb2e0watUmeulRoiSZ6XEdHYjtJ5KMZ+n5Yjp273o+piCrFZ4dhwilZWUy+HOdskvPsVAhqEPa0HhC59i0EqIUkn6XspF5PKMIpIHf8UChhVLmfGSPtFL6EleOtJ8shRTZSMhynStJNV/fT3Vm4zPN5IjNGKs1UuUpAiws1vSxU3F6LLV/FgA6sCZaUrUl9WSq7qEYqm1kihjz6HwpSBr0MoErUV7sFGdTa4XGQdYvxSGx2/ZI5swW8zZ0sDmaccXcpHQ11kveSJyEocdu7VQTq54FcmffBZccI2rnwXXToiCkEVcXYGbrHoigieYJFUFcjkJNh6tnV4BVxlZhamymRDlquyr3tzZ+GwjOUIjxlq9qKsGSQIJOb64BZ+1I14NSGJUHd1aYTXQtGlEXr9KvVBAK1rLqQzIltwLyPWyOURh8hCjBHCZtL1nbY8+7DzoUNBQrM/rLYWoXi14B46W1h60Ee1CM6VK3r6WsYCP7Vnh7dRzaCdEQZ4qhI7ATVYuIpGmCk3k+nyLKh/yVoAVs0wrY/L0AnbJVSbXyGJMlU2FqJ7q5aiah2rNAJD5wUZyhEaM9XoRh7U3VyWhoQISBebC8Qi5yu6noLh47Nb6VgVoWjVijSDhT6DvtO9aU91ag+eT60VMEGxZCklGQyGKtIqp8vIQRZwjNGJQL3vI9cIQVQeGKDKLqZIhqh2O0IhBvewh1wtDVB0YosgspkqGqHY4QiMG9bKHXC/9EEW2cXSIIh/DESHKWiZrMQEeF6K0E7KRcoiy0yLTV384R4eocF7iiH7JEZgqjwhR4YSsp7pGDOplD+InDFF1YYgis5gqGaKaorpGDOplD+InDFF1YYgis5gqGaKaorpGDOplD+InNUJU+J7+DHYr2Xe4fWxA9pX87KFSyrL226CVEOVP+kiC7IlRTsdvd0hXn3pPjHPVHRKfhKmSIaopqmvE2KCXhS5md9diVS2Wj857bxcWP6kXouRJUFgPww1fyHzJPV+4IDeX6R1k9hAUydV8+aMlUVHzpIwto6jiBby8Fgorb7O0EqLsUTQaqFRsqpEYdeRuv3S/noAESpmCBlelKVMDGgmt6Z19aJRswFTJENUU1TVibNCL+Z26ZFj+4ql5qN6ML5FJ8t0leyWF+BiE+7qw+EnFEKVHXRBFLikTApMLiqZHS/olewpIp4Dd7Zw11SbNhCh5cslQjGKy/aeeCJYYuapZauCeGZwAhk7WY6pkiGqK6hoxNuglOSPo+p0c4+M1he+XxqqRkt7IbV1Y/KRyiHo/cbAHFKZMkdADSSR0ezRRMhxFboMCSTEN006IgsxFYl0xqnX2nh0pZAbdvwq7xhGv35DbyyTbMFUyRDVFdY0YG/SSOePQ7+QI4J/IhJcjMVEyHu/qwuInNUIUSbQTokizmCoZopqiukYM6mUP4icMUXVhiCKzmCoZopqiukYM6mUP4icMUXVhiCKzmCoZopqiukYM6mUP4icMUXVhiCKzmCqrhyiyE4ao1sj10glRliYbYIgis5gq6y6IaI3sJ0izHlA0Q9Qe3FMYourAEEVmMVUesSCS1oCiGaL24J7CEFUHhigyi6mSIeonAEUzRO3BPYUhqg6NhCi7kxzI/U1zaMHpx3boHem8F6oSpkqGqJ8AFM0QtQf3FIaoOrS0i7JbnufQe3stVSof7pu2S/ZYsMiy9skAUyVD1E/AdE32wBBVk/ZCVHoqh95Vro/o1cd3+LNPXog8kkwhx0pqxU6IsmcnAiuQLsWn1pIlmCoZon4C0DLZDyTJEFWHBkOUHPUx8/rIvixT3+KTP/aEqqmSobzsonoF7AH2ZA2mSnM8QsgSGKLq0GiIsqcjdwOPfrqE3U988nGIWL2SlmmfRSETeIEQ4ex5lHqJLMJUyRBFyHIYourQUogijWKqZIgiZDkMUXVgiCKzmCoZoghZDkNUHRiiyCymSoYoQpbTCVFkJybMilAvnwQUiiNDFCHLSSGK7MeEWRG0aUsb+QwYoghZRQhRpE0Yoj4MhihCVsEQ1TQMUR8GQxQhq2CIahqGqA+DIYqQVTBENQ1D1IfBEEXIKhiimoYh6sNgiCJkFQxRTcMQ9WEwRBGyCoaopmGI+jAYoghZBUNU0zBEfRgMUYSsgiGqaRiiPgyGKEJWwRDVNAxRHwZDFCGrYIhqGgtR5JNgiCJkOQxRTYPljHweQbuEkDkYogghhDQKQxQhhBBCCCGErIP7KEIIIYQQQghZB/dRhBBCCCGEELKO/j7q/3jHKGmDW9xFRH8hpH2aXUy4gJBGYMAlZDm5v3AfRRqFyzohpAqdmBcJ55Fi5ja0nfRvolkuIKQRTvaRHG0z/Zvogv5CGqHjL+FvhGZKGiE302ahvxDSPraYDF+fyUu20os2ZG5bf1BLGyz8K8IFhDTCaT7SQxsv/CtCfyGNkBs/7LUDzZQ0QpU1+mjoL4S0jy0meB1mbuvYi7NwkoFMq/KfNaA8ammDhX+enye4gJBGOMdHelg7OAz/eX6eoL+QRjDjN2CdHZaZ6d/fYtJfv/78CxldZi63x78/v2TAv/+G8xoc0eaPIjfTZin6y5zq6T7z/Cj3oQCPphPzIuE8UszchraT/uXNWtpzGHBrQYPfyck+kqNtpn95F5b2nGX+sopPcy5yDh1/CX8jZTPtL1FVLC80OuCChbDCEjxogsv6TnIzbZaiv/RV3z+n+wwYNFGhzRkowIUUBRXbXdVNvTGtpdnFhAF3I4Mm6hlXUVCx3VXd1BvTCdw34K6jr5T9zvVDDeaHk/vL7D5qYCJiT8m0sstJ9/OW59W6BhMqhprezB9JxNZiGcn5G5oJraRLRhpA7A8FU6HygAcTVrTsSPtFEXXaDHTrZ1dmhvczue+ynqm+aBvpena5bI1lvFrXRqLtWE1vhu4zgALsXJkcXszJW1Im+3VCtWLh/hhHzgNJXutpdjFhwB1MWNGyI+0XRdRpM9Ctn12ZHF7MyVtSJvt1QrVi4f4YR84DSV6ncNuAW9KmijTpYJlSsstJ9n0dDYmdDooUu+sp2avNjE0bHDsPnGwwP5zcXxZ9HhU17Gpy7YUcNwE77yt7QCw/KNG5EHvxUr1uBucdQm271CvZrzgy4DiAUvPd9r3FVLTT5mCkMaN4eVD6R3LbZb1vTgPbiHYVc3r67lUf0jWejM6F2IuX6ptV/7xDqG2XeiX7FUcGHAdQar7bvreYinbaHIw0ZhQvD0oP6FbP6FyI4/dS/YYnO+pMsFeyX7Ez2UQcQKn5bvveYiraaXMw0phRvDwo7SOJ+IVhvx1mBtkZY+98puWVNLuYMOBmxAGUmu+27y2mop02ByONGcXLg9I+kohfGPbbYWaQnTH2zmdaPp7bBtye+lx3XV1HoY8pZaC1bnOuszLeSMBbn1FrqNfr2gv3O8/PZ1omB5P7y659VDKtbonB5QGxfM8GQsVQc9DMdLexcql2fwa989GG8/FNtT/ooHd90EG3wszwfia3Xdb72h5oc2AN3RJDY+kTy/cMJFQcs7npbmPlUu3+DHrnow3n45tqf9BB7/qgg26FmeEVKA0QxDGO9DrI6HQUK5dqz4xwtOF8fFPtDzroXR900K0wM7wOcRjWVr/kukH2hzUYpjcnpOz1NLuYMOAqpfFNtT/ooHd90EG3wszwOsRhWFv9kusG2R/WYJjenJCyT+G2Abcn5hEVRWnOKqVXYnB5jqhDqzEymni52/rs2AaD8eaElE2OJ/eXZfdH5coSXXWUlxgx3BFGGhmYSNZMNLM+2nG3vV+/rKjV7ttn7zzvqdwHCk613+1d83qj79aOhP5nhvczue2y3lf9wDbKtuDa7lcvM9JIqjVspmzaoeNuez/AfcpNeo+DIYDy5EJH3fY+R4CTNfv9dgsvHWQPvTSUQ2xlC80uJgy45T5QcKr9bu+a1xt9t3Yk9D85vMma/X67hdsx+A3cNuD2tNlXbpC5S7OvlLJORqsPKTfw+Qbzw8n9Zdk+qlWCkdF8PpHbLuu3ge6zEwrwLjS7mDDgkkZgwCVkObm/3GwfVdqdx30/+Sy4rFeH7rMTCvCmNLuYMOCSRmDAJWQ5ub+M7qP+85//WIKQM3HDu9eyTn8hpDXaX0y4gJBrYcAlZDlFf5nZR4Xc9eypS34sbnjgjst6yF3PnrqEkCHulaDZxYQLCLkQNzzAgEvING54IPcX7qNIQ7jhAS7rhJDNuFeCZhcTLiDkQtzwAAMuIdO44YHcXxbtoyxnIV7FEoQsB2bzAcu65SzEq1iCEFIF+FT7iwkXEHIhMBsGXEIWArMp+sv2fdTXF+p2/gHkexVLFHg/v74er287+X490snpYCh7Okf15zuknZ1t/mhgNp+6rNNfhI+Zi4w9+v736yWp6QbXdofyXFt2AZ9qfzHhArICOl1tYDYMuB1oY2QcmE3RX2BeHU410wCyzIbEmJ7Px5cVkJLC4/VyI0OJQGhD7Q/XlWiJwShjA4qW71ZPZ/E0NpDVDP2MdhSvl0ZL1gKz4bLeQewq2lgAWWZgYpN38pdPmou2ENOg12B2Pt1dSqeOYwOx/5I0yCzwqfYXEy4gQlYz9DPakadBr8HsfLq7lE4dxwZi/yVpfB4wGwbcLmI2tDFSBGZT9BeYV4flZmrACNxADa9iiT5iOtGMAsgyG8qs0POELB/JwKBKSueZcpL6W1K9O8JYvVTSL46NlqwEZvOpy7oBy/u5/vJJc3GkyrCYngUsc6yRmNZ2uvW11TFpkDngU+0vJlxAeiOM1UslHakyLKZnAcscaySmtZ1ufW11TBofB8yGAbcMbYwMgNkU/eX0fRSAzZglxZNoQbkxpULJDv26ZFmqV6WXiYR3tKp6rCYlLTlW0q8ORkvWA7Phst4ns0c7iebldmjpgQX69Vb8pVPWTrILefnm5/J+xRxvMSvmyXJ3qWnJTvkCruk5LnjvISXVuoXJBPCp9hcTLiCaiNWkpCVLJel0tYHZMOB2oI2RcWA2RX+5Yh+1lsw4yWcDs+GyvpdP8hf6PtkKfKr9xYQLCLkQmA0DLiELgdkU/WXvPmqIV7HEVnSLHuCa/lOA2Xz2sj7Eq1hiK5/kL/R9UgH4VPuLCRcQciEwGwZcQhYCsyn6y6J91Ab21CU/Fjc8cN9lfQN76hJChrhXgmYXEy4g5ELc8AADLiHTuOGB3F+4jyIN4YYHuKwTQjbjXgmaXUy4gJALccMDDLiETOOGB3J/4T6KNIQbHuCyTgjZjHslaHYx4QJCLsQNDzDgEjKNGx7I/YX7KNIQbniAyzohZDPulaDZxYQLCLkQNzzAgEvING54IPcX7qNIQ7jhAS7rhJDNuFeCZhcTLiDkQtzwAAMuIdO44YHcXy7aR8mj8wP1H5svTx5a3+r6WtkkAjvm4r8qkIPMn/WrAm54gMt64iP8hUZOzsS9EjS7mJyxgKTV44gH8dGpb4wbHmDADRzpL6ntCP3kRrjhgdxfzt9HyeuvjnkOMk6kynJfqxFGIy7rQz7JX2jk5DzcK0Gzi8nhC0jBvaY9bq0/0qlvjBseYMAVDvcXgw5yS9zwQO4vp++j8Cqwbz/+whC25cYlaVue5XLf5twKpVgol2r4VQOnvYU+r+4l88xYQfqe6MjIGkEyXolJrWjXO62lKprymlLeUqWJfzhueIDLugAj6JuA2IWbVjKjKbNxe5NiA0PzqwZOLd/Jq3vJPDNW6Fh4saN+1g80cnIO7pWg2cXk8AVEUCdzJxXvG3qcZ+ZXS2n8jQ0pfo4EnfpmuOEBBtzIof5iZCWzAjGJv9GBOiE1VYkpcipueCD3l6Y+j8oMBZnJUmJ+KumXsyop3TG41NnC6qmYEBsolQx0Tq1p76B7NTbd7UKJ/WQ1Bx19Pm54gMu6MrCVlJGZR9FsUkm/nFuUp7NMJL2zhdVTMSE2UCrpF709IS9JSDXcK0Gzi8nxC0giemrmcQt9vJ/WeuGFJvLp1HfFDQ8w4PY4xl+M/lWceIvdq7HL+JdciBseyP3l+vujxmzLyzxer5hv1ois53PKiEMiNqBIxrB6LCODyJqKBeN7AiC72kmD3qm2mWYlV9GfNZeyUxfee7wo9YVs4j8FNzzAZT0RTQIkE1LTSuZRMJvW/AUJGjk5CfdK0OxicvwCEj0N9JxPTqd9PKv9fAY/Te15CTr1XXHDAwy4SrLvY/zFQG7HQaRUui5XF7xoJGfjhgdyf7loH7UGsbAbrcmw9b6D3Gj0F+OGB7isb+Nm/kLIMbhXgmYXkwYXEPJzcMMDDLiXwReNN8END+T+coN9FPk5uOEBLuuEkM24V4JmFxMuIORC3PAAAy4h07jhgdxfuI8iDeGGB7isE0I2414Jml1MuICQC3HDAwy4hEzjhgdyf5nZRxFyMrde1gkh7dD+YsIFhFwLAy4hyyn6Cz+PIg3hhgfuuKyH3PXsqUsIGeJeCZpdTLiAkAtxwwMMuIRM44YHcn/hPoo0hBse4LJOCNmMeyVodjHhAkIuxA0PMOASMo0bHsj9hfso0hBueIDLOiFkM+6VoNnFhAsIuRA3PMCAS8g0bngg95fz91H5Ix3lEc3jD8IvPvxx1RMhs8LS1VhPU9fImbjhAS7rATFPt3n5CYnlDlCbVd43QMYe/ez79ZLUdINru0P5oR/vGzO5Le6VoNnF5PAF5Ayn8/JIrw3oQ9YOgGzHDQ8w4CrL7bkitPl74IYHcn+5dh8FsmVekoZlaMl3yNQ6ataGFUlV3Nqllv6EGTKyvvJuOx2lNsP10jBigyODfIU2son1yw0zyBA3PMBlXRCz6RkMsszQypaZ/whmMu7QRtFcNTP97K6i5bvV01k8jQ1kNUM/ox15GvQazM6nu0vp1HFsIPZfkgb5UbhXgmYXk1MC7tFOl/uXXA1O2CnYazk7D6W9nbGOSH3c8AADrrLQngvngxxpavBaVBNTL2uzAaT2vAu9WnjBSc7ADQ/k/tLCPkpPM4sRevakJ26amZFF6yoW8HRecrqjPVeL3RmFiqSAGx7gsi4MbUmyzOS6lulmmOcjGRhUSek8U05Sf0uqd0cYq5dKOuoOg2J6FrDMsUZiWtvp1tdWx6RBfhLulaDZxeT4gBs50OmyM/c9LZaQjrslcRawTL861hGpjxseYMBVFttzzz7lVN0rkTc1MG8FJwOXjOlug1MlyVm44YHcX67/Xl+0FEl3jSLLSSY1Un20wDAttTxX6RXbc9XSMppBsV4OKeCGB7isB8R2opd0DKlne6GQGJ/l+/Vkj70qvUwkvKNV1Ze54fsVc7zFrJgny92lpiU75Qu4pue44L2HlFTrFiY/BPdK0OxicvgCcobTeb4UCuWHFfKSniz0mxVTYkekPm54gAFXyc0P6Ql7DkT7TKYc6TVl6awdqeEBq1wy9D5TkpyEGx7I/YXPmSAN4YYHuKxvJK25hPxc3CtBs4sJAy65EDc8wIB7Ctz83Bg3PJD7C/dRpCHc8ACX9TXI5inCTRQh91hMGHDJhbjhAQbcU+A+6sa44YHcX7iPIg3hhge4rBNCNuNeCZpdTLiAkAtxwwMMuIRM44YHcn/hPoo0hBse4LJOCNmMeyVodjHhAkIuxA0PMOASMo0bHsj9hfso0hBueIDLOiFkM+6VoNnFhAsIuRA3PMCAS8g0bngg9xfuo0hDuOEBLuuEkM24V4JmFxMuIORC3PAAAy4h07jhgdxfuI8iDeGGB7isE0I2414Jml1MuICQC3HDAwy4hEzjhgdyfzl/HyUPxe8+UYwPMCEBNzzAZV3pecc2Zzna6YbtC/oMwYW9xPFInVhFW5WzPHOKupMi98a9EjS7mHABUYbtC1xAjsYNDzDgKmY0RhvGsNR6c/JZ1JvG8pFsGfMNcMMDub9cso96vN75T+v9uMWLjOGGB7isKz3v2OYsRzudtPZ8dn8uE3nP5+JehuPZMMK6kyL3xr0SNLuYcAFRpDUuIOfjhgcYcJXcBg61h+nGd3bdrS67msG7FPU5VFxN4IYHcn+5aB8lsh4mnPxS3E1nSU2bWWR1389eK+R2uOEBLutKZuFCPMXfsbeHgyMgJ8sKyWHCyS+tdbqQgz9hUFi4Q4aVTJWzd6pSpuSlzF5iJDOb5qARQu6xmHAB0VTIwZ+0SoQMK5kqcwGpiBseYMBVkoUkQ1MjSXbTMyFkBNM0BgUkJ7P9lIrFEp6ZX80zY1ddL5hrXErrhSy/PPhs0gFpNNXJO7VkZyRetVNy2ZhvgBseyP3lwn0UMO3GHNe1pFzcXriUTlXIJ+CGB7isK+4LStfg5cwWoYIjjPmOFR16EFJWZqzisIrjxbSR9L51uQpyw5i97dTCMJGlh12XGyHkHosJFxDFi2kjXEDOwg0PMOAqRRvIMocmpEj2OlcqtunmWKrSbRm5elIq2cnMKhbGJkj25sGnOqWS3QaRO1ryDrjhgdxfrt1HCapCyzFt4sw/4R8Td5aOlYSeAZC74YYHuKxHMhPPPSAQjb7vCGO+I9R2urwYrrkfFqt4Sb0esl6xpFfJ2yy2E6deaISQeywmXEC4gFyIGx5gwFVys3G6mT0Tivaz1pVCPSkQK2Rfbs2uZtULXlBsPNRW/KoyO/h+1lj7MtasmBBqbhzzDXDDA7m/nL+PImQUNzzAZZ0Qshn3StDsYsIFhFyIGx5gwCWLudPmpyJueCD3F+6jSEO44QEu64SQzbhXgmYXEy4g5ELc8AADLiHTuOGB3F+4jyIN4YYHZpf18FmxErJOh/5CSJu4V4LZxeQquICQC3HDA836SA79hVyIGx7I/WVmH0XIySxZ1nXj1P8HUHcn3j7opT1h2Cn9hZBmca+cWEyuhQsIuZb2fSSH/kKupegv6XWh0TPTkLuePXXJj8UND8AUdd8y9c/wtLWwGVSPnfYpZgL6CyFt4l4J8pjXFFxAyIW44YFmfSSH/kIuxA0P5P7Sf3VIMyUX4oYH3Exlh6T4ZsnSvX8AVXaS99hLe8KwU1Sx0dJfCGkK90rgrt0aXEDIhbjhgWZ9JIf+Qi7EDQ/k/pJeFxo0U3Ihbnhgdlm3nYwRsk6H/kJIm7hXgtnF5Cq4gJALccMDzfpIDv2FXIgbHsj95fx9VO+Biduenyi1Hp0fDKv7HEZrv9cgMsHCXobjkerZgJdQd1I3wA0PcFkXYAJdKy+Z0LSdxKvyMw6x9vfrJanlBpaX7KUnzHp5+4RUxr0SNLuYHB9wjaqeyJXkU3DDAwy4Ss/Sthme1OIL1M/DDQ/k/tLOPir9WFfQZT9DSuovf+E01MraSkmkAqEhvfQOrSHPG07jKPX1+sYxNKElLCdWSlVSIc8c/CwgjgHNLbYTmyk08lNwwwNc1hWxhmAZ0R6TMaULubFpXtkUc3tKzUw067+1F5BLWS9CGmG3hX77BVMn5DDcK0Gzi8nxC4iR+6z7dddLg1NqSawZSn81CaWkEleSD8ANDzDgKj2b9NOBzfUzpCRfoH42bngg95dm9lGqm3QhMwJBNJhXzNJI9q4iGbCckYqenugLl/QvztWI8irRqvxqLBzz7GSYyNLDrsuN/BTc8ACX9UAwicwyYBcByxnY2LQVqdV17NkYbXYiDZYMrGTqhByJeyVodjE5YwERxv13wmc9rc6b1YlwJbk5bniAAVfp2WQ87blAwQjzilkayd5VJAOWM1LR0xN9RY/BeccNpUpyjHA1Fo55djJMZOlh1+VGfgpueCD3lwvuj4LsXfiSTuoWMsPoqSjXWVd/Qdmxnl2RTEuNVfQ0EilXyYrJxfwkVQkDl44smTJ1RF6yl+hlplyl2MhPwQ0PcFl3zMCDKbjtFCy8Y1d9K3q/9FLIs6xkk1PNTqejI8y3kE4IORr3StDsYnLOAjLwxJie91lPCygl51xJPgU3PMCAa+TmlRllILhAwQh7JppdFDPmC9RPwA0P5P5ywT6KkDHc8ACXdULIZtwrQbOLCRcQciFueIABl5Bp3PBA7i/cR5GGcMMDXNYJIZtxrwTNLiZcQMiFuOEBBlxCpnHDA7m/cB9FGsIND3BZJ4Rsxr0SNLuYcAEhF+KGBxhwCZnGDQ/k/sJ9FGkINzzAZZ0Qshn3StDsYsIFhFyIGx5gwCVkGjc8kPsL91GkIdzwAJd1Qshm3CtBs4tJvoCELEJOhAGXkIW44YHcX7iPIg3hhge4rBNCNuNeCZpdTPIFJGQRciIMuIQsxA0P5P7CfRRpCDc8wGWdELIZ90rQ7GKSLyAhi5ATYcAlZCFueCD3l7P3UfLw+S51nj+f2u0+7X8dndH9rAfjt4EbHuCyDmCRHTvEecHAY6n0kxQ53kbHvJf5SbnusA9CWsO9EjS7mOQLSMgi5EQYcHM6cU6pE+xSu3yBemPc8EDuL1d9HuWvz2pQaGx5+3nJbq30+2VDqo6fRNzwAJd1oWOEsLqiQU5bo1/dYLQjdekapHncK0Gzi0m+gIQsQk6EAbdE1ShWaGx5+3nJbi1G4dNxwwO5v7Swj0J6qHMvIFeDtUhSU0j07Udsygtm1XM6bcbLecleraGd5i2Uar2feX2yFjc8wGXdgBW6zc1ZY54IhdUxUmbELmpOSvZT3bqWMOgapHXcK0Gzi0m+gIQsQk6EAbdEL4p1gp+Sx7tB5ESiGx1DLI0Fs+o5nTbj5bxkrxaj8Nm44YHcXxrZR8V0MgykLHOkZDTMnrnGBra12Wk/VZxuIV0le3HDA1zWI2p172h6U9YYE35FGFxN5Dlr6voYpgaTXyXkbNwrQbOLSb6AhCxCToQBt0QpMoLpeNeNklKWL1A/Czc8kPtLY/so1bnweD4nTAp/A2Yf6dwNJmTJ6XSbecmsnaxAqYW8VrouxAGQDbjhAS7rCTEwN8gJe84MO9ry4/XKriY0Kyu/oq5XAXQN0ijulaDZxSRfQEIWISfCgFtiJDJOBd8sHUIgsJiXzj0Ihiw5nW4zL5m1kxVgFD4NNzyQ+8tV+yhCCrjhAS7rhJDNuFeCZheTfAEJWYScCAMuIQtxwwO5v3AfRRrCDQ9wWSeEbMa9EjS7mOQLSMgi5EQYcAlZiBseyP2F+yjSEG54gMs6IWQz7pWg2cUkX0AIuQSzQAZcQqZxwwO5v8zsowg5E1jgrZd1Qkg7tL+Y+AJCyLUw4BIyDSywGFOm9lEhi5ATaf+lT05vWQ+569lTlxAyxL0S3GIxIYRMwxeo5FqKMYX7KNIW93rpk/sLjiF3PXvqEkKGuFeCWywmhJBp+AKVXEsxpnAfRdriXi99cn/BMeSuZ09dQsgQ90pwi8WEEDINX6CSaynGFO6jSFvc66VP7i84htz1zNXt/KDEXuTXJOKvSHy/XpJa3n5espee+G2KquMnZAHuleAWiwkhZJo84BJyCWaBeUzhPoq0RdFMmyX3FxxD7nrm6vZ3L8/no//LfGEPoyVfITfWSb/Dp6WkUratSW2ky0bWpvaYETrPd0dpf9Ztod9+fziEHIB7JbjFYkIImcYDLiHXkscU7qNIW3AfVSLfsXR3L2mTYpmlkrpvyepENF+3Muvb7KcBmounEy2kTZTCnRQ5BvdKcIvFhBBCyO3gPoq0BfdRJUZ2L56Uzclgx9JJC2Gj837FTK+2rc1eOu6J5ltIJ4QchHsl4D6KEELIEXAfRdqC+yhCyH7cKwH3UYQQQo6A+yjSFtxHEUL2414JuI8ihBByBNxHkbbgPooQsh/3SsB9FCGEkCOY2kcRcglmgdxHEUI2414JuI8ihBByBKP7KEKu5Xb7qJBFCGkD7qMIIYQcSn8fRQhZDvdRhDQL91GEEEIOhfsoQrbDfRQhzcJ9FCGEkEPhPoqQ7XAfRUizcB9FCCHkULiPImQ73EcR0izcRxFCCDkU7qMI2Q73UYQ0C/dRhBBCDoX7KEK2w30UIc3CfRQhhJBD4T6KkO1wH0VIs3AfRQgh5FC4jyJkO9xHEdIs3EcRQgg5FO6jCNkO91GENAv3UYQQQg6F+yhCtsN9FCHNwn0UIYSQQ+E+ipDt5PsoQkhrmHtyH0UIIeQIuI8iZDu+jyKENAv3UYQQQo6A+yhCCCGEEEIIWQf3UYQQQgghhBCyhv/97/8BYK/cGpjUXxkAAAAASUVORK5CYII=");
                //// Create a memory stream from the Base64 string
                //var imageStream = Base64ToMemoryStream(base64);
                //// Add the image to the document
                //Image i = document.AddImage(imageStream);

                //// Create a picture i.e. (A custom view of an image)
                //Picture p = i.CreatePicture();

                //// Create a new Paragraph.
                //var par = document.InsertParagraph();

                //// Append content to the Paragraph.
                //par.Append("Here is a cool picture")
                //   .AppendPicture(p)
                //   .Append(" don't you think so?");


                // TODO: Can this be converted to an LINQ expression as well?
                using (XmlReader reader = XmlReader.Create(new StringReader(html)))
                {
                    reader.ReadStartElement("html");
                    reader.ReadStartElement("body");


                    while (reader.Name != "body")
                    {
                        if (reader.Name == "img")
                        {
                            reader.Read();
                            // TODO: Add images
                        }

                        XElement el = (XElement)XNode.ReadFrom(reader);

                        if(el.Name.LocalName == "table")
                        {

                            document.InsertTable(GetTableByHtml(document, el.ToString()));
                        }
                        else
                        {
                            var p = document.InsertParagraph(String.Concat(el.Nodes()));
                            p.StyleName = SetStyleOfParagraphByElement(el.Name.LocalName);
                        }
                        
                        //Debug.WriteLine(String.Concat(el.Nodes()));
                    }

                    reader.ReadEndElement();
                }


                // Save all changes made to this document.
                document.Save();
            }
        }

        /// <summary>
        /// Sets the style of paragraph by element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private static string SetStyleOfParagraphByElement(string element)
        {
            switch (element)
            {
                case "h1":
                    return "Heading1";
                case "h2":
                    return "Heading2";
                case "h3":
                    return "Heading3";
                case "p":
                    return "Normal";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get a MemoryStream object from a Base64 image string.
        /// </summary>
        /// <param name="base64String">The string without the "data:image/png;base64," bit.</param>
        /// <returns></returns>
        private MemoryStream Base64ToMemoryStream(string base64String)
        {
            try
            {
                // Convert Base64 String to byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);
                return new MemoryStream(imageBytes, 0, imageBytes.Length);
            }
            catch(System.FormatException)
            {
                throw new ArgumentException("The image still has 'data:image/png;base64,'.");
            }
        }

        /// <summary>
        /// Gets the base64 from data URI.
        /// </summary>
        /// <param name="base64String">The base64 string.</param>
        /// <returns></returns>
        private static string GetBase64FromDataUri(string base64String)
        {
            // Remove the 'data:image/png;base64,' bit from an image string.
            return base64String.Split(',')[1];
        }

        /// <summary>
        /// Gets the DocX table by HTML.
        /// </summary>
        /// <param name="document">The DocX document.</param>
        /// <param name="tableHtml">The table HTML.</param>
        /// <returns></returns>
        private Table GetTableByHtml(DocX document, string tableHtml)
        {
            var tableList = HtmlTableToList(tableHtml);
            var table = document.AddTable(tableList.Count, 2);

            var rowCount = 0;
            foreach (var tr in tableList)
            {
                var dataCount = 0;
                foreach (var td in tr)
                {
                    table.Rows[rowCount].Cells[dataCount].Paragraphs.First().Append(td);
                    dataCount++;
                }
                rowCount++;
            }

            return table;
        }

        /// <summary>
        /// Turn a html table into a two dimensional List.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        private List<List<string>> HtmlTableToList(string html)
        {
            var result = new List<List<string>>();

            var doc = XDocument.Parse(html);
            var rows = from @table in doc.Descendants("table")
                       from row in @table.Descendants("tr")
                       from data in row.Descendants("td")
                        select new
                        {
                            Elements = data.Descendants()
                        };


            foreach (var row in rows)
            {
                var td = new List<string>();

                foreach (var element in row.Elements)
                {
                    td.Add(element.ToString());
                    Debug.WriteLine(element.ToString());
                }

                result.Add(td);
            }

            return result;
        }
    }
}
