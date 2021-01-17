namespace EntitiesBT.Variant
{

// public class SingleLocalVariableProperty : EntitiesBT.Variable.LocalVariableProperty<System.Single>, SingleProperty { }

public interface Int32PropertyReader : IVariantReader<System.Int32> { }
public class Int32LocalVariablePropertyReader : LocalVariant.Any<System.Int32>, Int32PropertyReader { }

public interface Int64PropertyReader : IVariantReader<System.Int64> { }
public class Int64LocalVariablePropertyReader : LocalVariant.Any<System.Int64>, Int64PropertyReader { }

public interface IFloat2PropertyReader : IVariantReader<Unity.Mathematics.float2> { }
public class Float2NodeVariantReader : NodeVariant.Any<Unity.Mathematics.float2>, IFloat2PropertyReader { }
public class Float2LocalVariablePropertyReader : LocalVariant.Any<Unity.Mathematics.float2>, IFloat2PropertyReader { }

public interface IFloat2PropertyWriter : IVariantWriter<Unity.Mathematics.float2> { }

public interface IFloat3PropertyReader : IVariantReader<Unity.Mathematics.float3> { }
public class Float3NodeVariantReader : NodeVariant.Any<Unity.Mathematics.float3>, IFloat3PropertyReader { }
public class Float3LocalVariablePropertyReader : LocalVariant.Any<Unity.Mathematics.float3>, IFloat3PropertyReader { }

public interface IFloat3PropertyWriter : IVariantWriter<Unity.Mathematics.float3> { }

public interface IQuaternionPropertyReader : IVariantReader<Unity.Mathematics.quaternion> { }
public class QuaternionNodeVariantReader : NodeVariant.Any<Unity.Mathematics.quaternion>, IQuaternionPropertyReader { }
public class QuaternionLocalVariablePropertyReader : LocalVariant.Any<Unity.Mathematics.quaternion>, IQuaternionPropertyReader { }

public interface IQuaternionPropertyWriter : IVariantWriter<Unity.Mathematics.quaternion> { }

}

