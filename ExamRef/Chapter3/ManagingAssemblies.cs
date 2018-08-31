namespace Chapter3
{
    class ManagingAssemblies
    {
    }
}

/*Example 3-29. Inspecting public key of signed assembly, command to be run in Visual Studios command prompt
 * C:\>sn -Tp C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.Data.dll
 * 
 * Example 3-30 Redirecting assembly bindings to a newer version:
 * 
 *  <configuration> 
        <runtime> 
            <assemblyBinding xmLns:"urn:schemas-microsoft-com:asm.v1"> 
                <dependentAssembly> 
                    <assemblyIdentity name="myAssembly" publicKeyToken="32ab4ba45e0a69a1" culture="en-us" /> 
                    <!-- Redirecting to version 2.0.0.0 of the assembly. --> 
                    <bindingRedirect oldVersion:"1.0.0.0" newVersion="2.0.0.0"/>
                </dependentAssembly>
            </assemblyBinding>
        </runtime>
    </configuration>

Example 3-31 Specifying additional locations for assemblies

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <runtime>
            <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
                <probing privatePath="MyLibraries"/>
            </assemblyBinding>
        </runtime>
    </configuration>

Example 3-32 Specifying additional locations for assemblies on the web

    <?xml version="1.0" encoding="utf-8" ?>
    <configuration>
        <runtime>
            <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
                <dependentAssembly>
                    <codeBase version="1.0.0.0" href="http://www.mydomain.com/ReferenceAssembly.dll"/>
                </dependentAssembly>
            </assemblyBinding>
        </runtime>
    </configuration>
 * 
 * Thought experiment: You are discussing the reasons why you want to sign an assembly that you have built.
 * The assembly will be distributed with a desktop application you are building. The assembly won't be
 * shared by other applications.
 * 
 * 1. Should you sign the assembly? Meh, doesn't matter much in this case, right?
 * 2. If the assembly is signed you know the publisher had access to the private key, so there's less chance
 * it is a spoof of the original .dll in.
 * 
 * Review:
 * 1. You are building a strong-named assembly and you want to reference a regular assembly to reuse some code you built.
 *  What do you do?
 *      3. You need to sign the other assembly before using it.
 * 2. You are building an assembly that will be used by a couple of server applications. You want to make the update process
 *  of this assembly as smooth as possible. Which steps should you take?
 *      2. Deploy assembly to GAC.
 *      3. Strongly name the assembly
 * 
 * 3. You want to deploy an assembly to a shared location on the intranet, which steps should you take?
 *      1. Strong name it
 *      2. Use the codebase configuration element in the applications that use the assembly.
 *      4. Use the assemblyBinding configuration element with the probing option
 * */


